using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using DataAccess;
using DataStructures;
using DataStructures.Enums;
using DataStructures.POCO;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using MoreLinq;

using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ILogger = DataStructures.ILogger;

namespace OverviewApp.ViewModels
{
    public class AccountViewModel : MyBaseViewModel
    {
        #region Fields

        

        private readonly List<Equity> filteredEquity = new List<Equity>();
        private readonly List<Equity> filteredEquityByDate = new List<Equity>();
        private readonly Timer timer;
        private readonly Timer timerEquity;
        private ObservableCollection<string> accounts;

        private ObservableCollection<PortfolioSummary> accsummaryCollection;
        private bool canRemoveAccountFilter;
        private bool canRemoveEndDateFilter;
        private bool canRemoveStartDateFilter;

        private ObservableCollection<Equity> equityCollection;

        private ObservableCollection<TradeHistory> historytradesCollection;

        private ObservableCollection<LiveTrade> livetradesCollection;

        private ObservableCollection<OpenOrder> openordersCollection;
        private string selectedAccount;
        private DateTime selectedEndDate = DateTime.Today;
        private DateTime selectedStartDate = DateTime.Today.AddDays(-10);
#pragma warning disable CS0169 // The field 'Account_ViewModel.lastUpdateMilliSeconds' is never used
        private long lastUpdateMilliSeconds;
#pragma warning restore CS0169 // The field 'Account_ViewModel.lastUpdateMilliSeconds' is never used

        private PlotModel plotModel;

#pragma warning disable CS0169 // The field 'Account_ViewModel.stopwatch' is never used
        private Stopwatch stopwatch;
#pragma warning restore CS0169 // The field 'Account_ViewModel.stopwatch' is never used

        #endregion

        #region

        /// <summary>
        ///     Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        public AccountViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            InitializeCommands();
            PlotModel = new PlotModel();
            DataService = dataService;
            
            SetUpModel();
            LoadData();

            timer = new Timer();
            timer.Elapsed += timer_tick;
            timer.Interval = 1000; //10000 ms = 10 seconds
            timer.Enabled = true;

            timerEquity = new Timer();
            timerEquity.Elapsed += timer_tick_equity;
            timerEquity.Interval = 65000;
            timerEquity.Enabled = true;

            logger.Log(LogType.Admin, "Ahoj", LogSeverity.Info);

            // This will register our method with the Messenger class for incoming
            // messages of type ViewCollectionViewSourceMessageToken.
            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the IDownloadDataService member
        /// </summary>
        internal IDataService DataService { get; set; }

        /// <summary>
        ///     Gets or sets the CollectionViewSource which is the proxy for the
        ///     collection of Things and the datagrid in which each thing is displayed.
        /// </summary>
        private CollectionViewSource Cvs { get; set; }

        private CollectionViewSource Tcvs { get; set; }
        private CollectionViewSource Ocvs { get; set; }
        private CollectionViewSource Scvs { get; set; }
        private CollectionViewSource Ecvs { get; set; }

        public RelayCommand ResetDateFilterCommand { get; private set; }
        public RelayCommand ResetFiltersCommand { get; private set; }
        public RelayCommand RemoveAccountFilterCommand { get; private set; }

        public ObservableCollection<LiveTrade> LiveTrades
        {
            get { return livetradesCollection; }
            set
            {
                if (Equals(value, livetradesCollection)) return;
                livetradesCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TradeHistory> TradesHistory
        {
            get { return historytradesCollection; }
            set
            {
                if (Equals(value, historytradesCollection)) return;
                historytradesCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OpenOrder> OpenOrders
        {
            get { return openordersCollection; }
            set
            {
                if (Equals(value, openordersCollection)) return;
                openordersCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<PortfolioSummary> AccountSummaryCollection
        {
            get { return accsummaryCollection; }
            set
            {
                if (Equals(value, accsummaryCollection)) return;
                accsummaryCollection = value;
                RaisePropertyChanged();
            }
        }

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Equity> EquityCollection
        {
            get { return equityCollection; }
            set
            {
                if (Equals(value, equityCollection)) return;
                equityCollection = value;
                //PlotModel.InvalidatePlot(true); //UpdateModel();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets a list of accounts which is used to populate the account filter
        ///     drop down list.
        /// </summary>
        public ObservableCollection<string> Accounts
        {
            get { return accounts; }
            set
            {
                if (accounts == value)
                    return;
                accounts = value;
                //RaisePropertyChanged("Authors");
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the selected account in the list to filter the collection
        /// </summary>
        public string SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                if (selectedAccount == value)
                    return;
                selectedAccount = value;
                //RaisePropertyChanged("SelectedAuthor");
                RaisePropertyChanged();
                // FilteredEquity.Clear();

                ApplyFilter(!string.IsNullOrEmpty(selectedAccount) ? FilterField.Account : FilterField.None);
                UpdateModel();
            }
        }

        /// <summary>
        ///     Gets or sets the selected account in the list to filter the collection
        /// </summary>
        public DateTime SelectedStartDate
        {
            get { return selectedStartDate; }
            set
            {
                if (selectedStartDate == value)
                    return;
                selectedStartDate = value;
                //RaisePropertyChanged("SelectedAuthor");
                RaisePropertyChanged();
                if (selectedStartDate != DateTime.Today.AddYears(-1))
                {
                    // FilteredEquityByDate.Clear();
                    ApplyFilter(FilterField.StartDate);
                    //UpdateModelByStartDate();
                    UpdateModel();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the selected account in the list to filter the collection
        /// </summary>
        public DateTime SelectedEndDate
        {
            get { return selectedEndDate; }
            set
            {
                if (selectedEndDate == value)
                    return;
                selectedEndDate = value;
                //RaisePropertyChanged("SelectedAuthor");
                RaisePropertyChanged();
                if (selectedEndDate != DateTime.Today)
                {
                    filteredEquityByDate.Clear();
                    ApplyFilter(FilterField.EndDate);
                    //UpdateModelByEndDate();
                    UpdateModel();
                }

                // UpdateModel();
            }
        }

        public bool CanRemoveAccountFilter
        {
            get { return canRemoveAccountFilter; }
            set
            {
                canRemoveAccountFilter = value;
                //RaisePropertyChanged("CanRemoveAuthorFilter");
                RaisePropertyChanged("CanRemoveAccountFilter");
            }
        }

        public bool CanRemoveEndDateFilter
        {
            get { return canRemoveEndDateFilter; }
            set
            {
                canRemoveEndDateFilter = value;
                //RaisePropertyChanged("CanRemoveAuthorFilter");
                RaisePropertyChanged("CanRemoveEndDateFilter");
            }
        }

        public bool CanRemoveStartDateFilter
        {
            get { return canRemoveStartDateFilter; }
            set
            {
                canRemoveStartDateFilter = value;
                //RaisePropertyChanged("CanRemoveAuthorFilter");
                RaisePropertyChanged("CanRemoveStartdDateFilter");
            }
        }

        public DateTime Today
        {
            get { return DateTime.Today; }
            private set { }
        }

        #endregion

        #region Nested

       

        #endregion

        /// <summary>
        ///     This method handles a message recieved from the View which enables a reference to the
        ///     instantiated CollectionViewSource to be used in the ViewModel.
        /// </summary>
        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken token)
        {
            if (token.LiveTradesCollectionViewSource != null)
            {
                Cvs = token.LiveTradesCollectionViewSource;
            }
            else if (token.TradesHistoryCollectionViewSource != null)
            {
                Tcvs = token.TradesHistoryCollectionViewSource;
            }
            else if (token.OpenTradesCollectionViewSource != null)
            {
                Ocvs = token.OpenTradesCollectionViewSource;
            }
            else if (token.EquityCollectionViewSource != null)
            {
                Ecvs = token.EquityCollectionViewSource;
            }
            else
            {
                Scvs = token.AccountSummaryCollectionViewSource;
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<ViewCollectionViewSourceMessageToken>(this);
            base.Cleanup();
        }

        /// <summary>
        ///     Loads the data.
        /// </summary>
        private void LoadData()
        {
            var livetrades = DataService.GetLiveTrades();
            var history = DataService.GetTradeHistory();
            var openorder = DataService.GetOpenOrders();
            var summary = DataService.GetPortfolioSummary();
            var equity = DataService.GetEquity();
            var q1 = from t in history
                select t.Account;
            var q2 = from t in livetrades
                select t.Account;
            var q3 = from t in openorder
                select t.Account;
            var q4 = from t in summary
                select t.Account;
            var dis = q1.Union(q2).Union(q3).Union(q4);

            Accounts = new ObservableCollection<string>(dis);

            LiveTrades = new ObservableCollection<LiveTrade>(livetrades);
            TradesHistory = new ObservableCollection<TradeHistory>(history);
            OpenOrders = new ObservableCollection<OpenOrder>(openorder);
            AccountSummaryCollection = new ObservableCollection<PortfolioSummary>(summary);
            EquityCollection = new ObservableCollection<Equity>(equity);
            //SetUpModelData();

            //EquityCollection = EquityCollection1.GroupBy(m => m.Account).OrderBy(m => m.Key).ToList();
        }

        /// <summary>
        ///     Updates the data.
        /// </summary>
        private void UpdateData()
        {
            var livetrades = DataService.GetLiveTrades();
            var openorder = DataService.GetOpenOrders();
            var summary = DataService.GetPortfolioSummary();
            ObservableCollection<TradeHistory> history;
            if (TradesHistory.Count > 0)
            {
                var lastIdHistoricalTrade = TradesHistory[TradesHistory.Count - 1].Id;
                history = DataService.GetTradeHistory(lastIdHistoricalTrade);
            }
            else
            {
                history = DataService.GetTradeHistory();
            }

            foreach (var tradeHistory in history)
            {
                Application.Current.Dispatcher.Invoke(() => { TradesHistory.Add(tradeHistory); });
            }

            LiveTrades = new ObservableCollection<LiveTrade>(livetrades);
            Application.Current.Dispatcher.Invoke(() => { OpenOrders.Clear(); });

            OpenOrders = new ObservableCollection<OpenOrder>(openorder);
            AccountSummaryCollection = new ObservableCollection<PortfolioSummary>(summary);
        }

        /// <summary>
        ///     Updates the equity data.
        /// </summary>
        private void UpdateEquityData()
        {
            ObservableCollection<Equity> equity;
            if (EquityCollection.Count > 0)
            {
                var lastIdEquity = EquityCollection[EquityCollection.Count - 1].Id;
                equity = DataService.GetEquity(lastIdEquity);
            }
            else
            {
                equity = DataService.GetEquity();
            }
            foreach (var equit in equity)
            {
                Application.Current.Dispatcher.Invoke(() => { EquityCollection.Add(equit); });
            }
        }

        /// <summary>
        ///     Sets up model.
        /// </summary>
        private void SetUpModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;

#pragma warning disable CS0612 // 'DateTimeAxis.DateTimeAxis(AxisPosition, string, string, DateTimeIntervalType)' is obsolete
            var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Date", "HH:mm")
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                StringFormat = "HH:mm dd/WW/yy",
                IntervalLength = 80,
                ToolTip = "datetime",
                LabelFormatter = d => DateTimeAxis.ToDateTime(d).ToString("HH:mm:ss\ndd/MM/yy")
            };
#pragma warning restore CS0612 // 'DateTimeAxis.DateTimeAxis(AxisPosition, string, string, DateTimeIntervalType)' is obsolete
            PlotModel.Axes.Add(dateAxis);
#pragma warning disable CS0612 // 'LinearAxis.LinearAxis(AxisPosition, double, double, string)' is obsolete
            var valueAxis = new LinearAxis(AxisPosition.Left, 0)
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Value"
            };
#pragma warning restore CS0612 // 'LinearAxis.LinearAxis(AxisPosition, double, double, string)' is obsolete
            PlotModel.Axes.Add(valueAxis);
        }

        /// <summary>
        ///     Updates the model.
        /// </summary>
        private void UpdateModel()
        {
            var xxx = Ecvs.CollectionViewType;
            var xxxx = Ecvs.GetType();
            var listEquity = Ecvs.View.Cast<Equity>().ToList();
            if (listEquity.Count != 0)
            {
                var dataperaccount =
                    listEquity.DistinctBy(x => x.Id).GroupBy(m => m.Account).OrderBy(m => m.Key).ToList();

                var min = listEquity.MinBy(m => m.Value).Value;
                var max = listEquity.MaxBy(m => m.Value).Value;

                PlotModel.Series.Clear();
                var find = PlotModel.Axes.First(x => x.Title == "Value");
                PlotModel.Axes.Remove(find);
#pragma warning disable CS0612 // 'LinearAxis.LinearAxis(AxisPosition, double, double, string)' is obsolete
                var valueAxis = new LinearAxis(AxisPosition.Left, 0)
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    Maximum = max,
                    Minimum = min,
                    //FilterMaxValue = max+1000,
                    //FilterMinValue = min+1000,
                    //StartPosition = min,
                    //EndPosition = max,

                    Title = "Value"
                };
#pragma warning restore CS0612 // 'LinearAxis.LinearAxis(AxisPosition, double, double, string)' is obsolete
                PlotModel.Axes.Add(valueAxis);

                foreach (var data in dataperaccount)
                {
                    var lineSerie = new LineSeries
                    {
                        StrokeThickness = 2,
                        MarkerSize = 3,
                        //MarkerStroke = colors[data.Key],
                        //MarkerType = markerTypes[data.Key],
                        TrackerFormatString =
                            "Date: {2:HH.mm dd.MM.yy}" + Environment.NewLine + "Value: {4}" + Environment.NewLine +
                            "Account: {0}",
                        CanTrackerInterpolatePoints = true,
                        Title = $"Account {data.Key}",
                        Smooth = false
                    };

                    data.ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.UpdateTime), d.Value)));
                    PlotModel.Series.Add(lineSerie);
                    PlotModel.Title = "Equity Graph"; // Account:"+data.Key;
                }
            }
            else
            {
                PlotModel.Series.Clear();
            }

            PlotModel.InvalidatePlot(true);
        }

        private void InitializeCommands()
        {
            RemoveAccountFilterCommand = new RelayCommand(RemoveAccountFilter, () => CanRemoveAccountFilter);
            ResetFiltersCommand = new RelayCommand(ResetFilters, null);
            ResetDateFilterCommand = new RelayCommand(ResetDateFilter, null);
        }

        private void timer_tick_equity(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Filters by account.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FilterEventArgs" /> instance containing the event data.</param>
        private void FilterByAccount(object sender, FilterEventArgs e)
        {
            // see Notes on Filter Methods:
            if (e.Item is LiveTrade)
            {
                var src = (LiveTrade) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account) != 0)
                    e.Accepted = false;
            }
            else if (e.Item is OpenOrder)
            {
                var src = (OpenOrder) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account) != 0)
                    e.Accepted = false;
            }
            else if (e.Item is TradeHistory)
            {
                var src = (TradeHistory) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account) != 0)
                    e.Accepted = false;
            }
            else if (e.Item is Equity)
            {
                var src = (Equity) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account) != 0)
                {
                    e.Accepted = false;
                }
                else
                {
                    filteredEquity.Add(src);
                }
            }
            else
            {
                var src = (PortfolioSummary) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account) != 0)
                {
                    e.Accepted = false;
                }
            }
        }

        /// <summary>
        ///     Adds the account filter.
        /// </summary>
        private void AddAccountFilter()
        {
            if (CanRemoveAccountFilter)
            {
                Tcvs.Filter -= FilterByAccount;
                Tcvs.Filter += FilterByAccount;
                Cvs.Filter -= FilterByAccount;
                Cvs.Filter += FilterByAccount;
                Ocvs.Filter -= FilterByAccount;
                Ocvs.Filter += FilterByAccount;
                Scvs.Filter -= FilterByAccount;
                Scvs.Filter += FilterByAccount;
                Ecvs.Filter += FilterByAccount;
                Ecvs.Filter -= FilterByAccount;
            }
            else
            {
                Tcvs.Filter += FilterByAccount;
                Ocvs.Filter += FilterByAccount;
                Cvs.Filter += FilterByAccount;
                Scvs.Filter += FilterByAccount;
                Ecvs.Filter += FilterByAccount;
                CanRemoveAccountFilter = true;
            }
        }

        /// <summary>
        ///     Adds the end date filter.
        /// </summary>
        private void AddEndDateFilter()
        {
            if (CanRemoveEndDateFilter)
            {
                Tcvs.Filter -= FilterByEndDate;
                Tcvs.Filter += FilterByEndDate;
                Ecvs.Filter -= FilterByEndDate;
                Ecvs.Filter += FilterByEndDate;
            }
            else
            {
                Tcvs.Filter += FilterByEndDate;
                Ecvs.Filter += FilterByEndDate;
                CanRemoveEndDateFilter = true;
            }
        }

        /// <summary>
        ///     Filters the date by end date.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FilterEventArgs" /> instance containing the event data.</param>
        private void FilterByEndDate(object sender, FilterEventArgs e)
        {
            if (e.Item is TradeHistory)
            {
                var src = (TradeHistory) e.Item;
                if (src == null)
                    e.Accepted = false;
                //else if (string.Compare(SelectedAccount, src.Account) != 0)
                else if (DateTime.Compare(SelectedEndDate, src.ExecTime) <= 0)
                    e.Accepted = false;
            }
            else if (e.Item is Equity)
            {
                var src = (Equity) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (DateTime.Compare(SelectedEndDate, src.UpdateTime) <= 0)
                {
                    e.Accepted = false;
                }
                else
                {
                    filteredEquityByDate.Add(src);
                }
            }
        }

        /// <summary>
        ///     Adds the start date filter.
        /// </summary>
        private void AddStartDateFilter()
        {
            if (CanRemoveStartDateFilter)
            {
                Tcvs.Filter -= FilterByStartDate;
                Ecvs.Filter -= FilterByStartDate;
                Tcvs.Filter += FilterByStartDate;
                Ecvs.Filter += FilterByStartDate;
            }
            else
            {
                Tcvs.Filter += FilterByStartDate;
                Ecvs.Filter += FilterByStartDate;
                CanRemoveStartDateFilter = true;
            }
        }

        /// <summary>
        ///     Filters the by start date.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FilterEventArgs" /> instance containing the event data.</param>
        private void FilterByStartDate(object sender, FilterEventArgs e)
        {
            if (e.Item is TradeHistory)
            {
                var src = (TradeHistory) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (DateTime.Compare(SelectedStartDate, src.ExecTime) >= 0)
                    e.Accepted = false;
            }
            else if (e.Item is Equity)
            {
                var src = (Equity) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (DateTime.Compare(SelectedStartDate, src.UpdateTime) >= 0)
                {
                    e.Accepted = false;
                }
                else
                {
                    filteredEquityByDate.Add(src);
                }
            }
        }

        /// <summary>
        ///     Resets the date filter.
        /// </summary>
        private void ResetDateFilter()
        {
            RemoveDateFilters();
        }

        /// <summary>
        ///     Resets all filters.
        /// </summary>
        private void ResetFilters()
        {
            RemoveAccountFilter();
        }

        /// <summary>
        ///     Removes the account filter.
        /// </summary>
        public void RemoveAccountFilter()
        {
            if (CanRemoveAccountFilter)
            {
                Cvs.Filter -= FilterByAccount;
                Ocvs.Filter -= FilterByAccount;
                Tcvs.Filter -= FilterByAccount;
                Scvs.Filter -= FilterByAccount;
                Ecvs.Filter -= FilterByAccount;

                SelectedAccount = null;
                CanRemoveAccountFilter = false;
                filteredEquity.Clear();
                PlotModel.Series.Clear();
                // SetUpModelData();
                PlotModel.InvalidatePlot(true);
            }
        }

        /// <summary>
        ///     Removes the date filters.
        /// </summary>
        private void RemoveDateFilters()
        {
            if (CanRemoveEndDateFilter)
            {
                Tcvs.Filter -= FilterByEndDate;
                Ecvs.Filter -= FilterByEndDate;
                //SelectedEndDate = new DateTime(2020, 1, 1);
            }
            if (CanRemoveStartDateFilter)
            {
                Tcvs.Filter -= FilterByStartDate;
                Ecvs.Filter -= FilterByStartDate;
                //SelectedStartDate = new DateTime(2014, 1, 1);
            }
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        ///     Applies the filter.
        /// </summary>
        /// <param name="field">The field.</param>
        private void ApplyFilter(FilterField field)
        {
            switch (field)
            {
                case FilterField.Account:
                    AddAccountFilter();
                    break;

                case FilterField.StartDate:
                    AddStartDateFilter();
                    break;

                case FilterField.EndDate:
                    AddEndDateFilter();
                    break;

                default:
                    break;
            }
        }
    }
}