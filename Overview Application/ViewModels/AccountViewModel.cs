using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using DataAccess;
using DataStructures;
using DataStructures.Enums;
using EntityData;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using MoreLinq;

using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using QDMS;
using ReactiveUI;


namespace OverviewApp.ViewModels
{
    public class AccountViewModel : MyBaseViewModel
    {
        #region Fields

        

        private readonly List<Equity> filteredEquity = new List<Equity>();
        private readonly List<Equity> filteredEquityByDate = new List<Equity>();
        private readonly Timer timer;
        private readonly Timer timerEquity;
        private ReactiveList<string> accounts;

        private ReactiveList<PortfolioSummary> accsummaryCollection;
        private bool canRemoveAccountFilter;
        private bool canRemoveEndDateFilter;
        private bool canRemoveStartDateFilter;

        private ReactiveList<Equity> equityCollection;

        private ReactiveList<TradeHistory> historytradesCollection;

        private ReactiveList<LiveTrade> livetradesCollection;

        private ReactiveList<OpenOrder> openordersCollection;
        private string selectedAccount;
        private DateTime selectedEndDate = DateTime.Today;
        private DateTime selectedStartDate = DateTime.Today.AddDays(-10);
#pragma warning disable CS0169 // The field 'Account_ViewModel.lastUpdateMilliSeconds' is never used
        private long lastUpdateMilliSeconds;
#pragma warning restore CS0169 // The field 'Account_ViewModel.lastUpdateMilliSeconds' is never used

        private PlotModel plotModel;

#pragma warning disable CS0169 // The field 'Account_ViewModel.stopwatch' is never used
        private Stopwatch stopwatch;
        private ReactiveList<Account> accountsList;
        private int latestProccesedCommissionMessage=0;
#pragma warning restore CS0169 // The field 'Account_ViewModel.stopwatch' is never used
        private MessageHandler messageHandler;
        #endregion

        #region

        /// <summary>
        ///     Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        public AccountViewModel(IMyDbContext context, ILogger logger) : base(context, logger)
        {
          
            InitializeCommands();
            PlotModel = new PlotModel();
           
            
            SetUpModel();
            messageHandler = new MessageHandler(Context);
            LoadData();

            timer = new Timer();
            timer.Elapsed += timer_tick;
            timer.Interval = 10000; //10000 ms = 10 seconds
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
        ///     Gets or sets the CollectionViewSource which is the proxy for the
        ///     collection of Things and the datagrid in which each thing is displayed.
        /// </summary>
        private CollectionViewSource LiveTradesCollectionViewSource { get; set; }

        private CollectionViewSource TradesHistoryCollectionViewSource { get; set; }
        private CollectionViewSource OpenTradesCollectionViewSource { get; set; }
        private CollectionViewSource AccountsCollectionViewSource { get; set; }
        private CollectionViewSource EquityCollectionViewSource { get; set; }

        public RelayCommand ResetDateFilterCommand { get; private set; }
        public RelayCommand ResetFiltersCommand { get; private set; }
        public RelayCommand RemoveAccountFilterCommand { get; private set; }

        public ReactiveList<LiveTrade> LiveTrades
        {
            get { return livetradesCollection; }
            set { this.RaiseAndSetIfChanged(ref livetradesCollection, value); }
        }

        public ReactiveList<TradeHistory> TradesHistory
        {
            get { return historytradesCollection; }
            set { this.RaiseAndSetIfChanged(ref historytradesCollection, value); }
        }

        public ReactiveList<OpenOrder> OpenOrders
        {
            get { return openordersCollection; }
            set { this.RaiseAndSetIfChanged(ref openordersCollection, value); }
        }

        public ReactiveList<PortfolioSummary> AccountSummaryCollection
        {
            get { return accsummaryCollection; }
            set { this.RaiseAndSetIfChanged(ref accsummaryCollection, value); }
        }

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { this.RaiseAndSetIfChanged(ref plotModel, value); }
        }

        public ReactiveList<Equity> EquityCollection
        {
            get { return equityCollection; }
            set { this.RaiseAndSetIfChanged(ref equityCollection, value); }
        }

        /// <summary>
        ///     Gets or sets a list of accounts which is used to populate the account filter
        ///     drop down list.
        /// </summary>
        public ReactiveList<string> Accounts
        {
            get { return accounts; }
            set { this.RaiseAndSetIfChanged(ref accounts, value); }
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
               
                //RaisePropertyChanged("SelectedAuthor");
             this.RaiseAndSetIfChanged(ref selectedAccount, value);
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
              
                //RaisePropertyChanged("SelectedAuthor");
                this.RaiseAndSetIfChanged(ref selectedStartDate, value);
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
                this.RaiseAndSetIfChanged(ref selectedEndDate, value);
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
            set { this.RaiseAndSetIfChanged(ref canRemoveAccountFilter, value); }
        }

        public bool CanRemoveEndDateFilter
        {
            get { return canRemoveEndDateFilter; }
            set { this.RaiseAndSetIfChanged(ref canRemoveEndDateFilter, value); }
        }

        public bool CanRemoveStartDateFilter
        {
            get { return canRemoveStartDateFilter; }
            set { this.RaiseAndSetIfChanged(ref canRemoveStartDateFilter, value); }
        }

        public DateTime Today => DateTime.Today;

        #endregion

      

        /// <summary>
        ///     This method handles a message recieved from the View which enables a reference to the
        ///     instantiated CollectionViewSource to be used in the ViewModel.
        /// </summary>
        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken token)
        {
            if (token.LiveTradesCollectionViewSource != null)
            {
                LiveTradesCollectionViewSource = token.LiveTradesCollectionViewSource;
            }
            else if (token.TradesHistoryCollectionViewSource != null)
            {
                TradesHistoryCollectionViewSource = token.TradesHistoryCollectionViewSource;
            }
            else if (token.OpenTradesCollectionViewSource != null)
            {
                OpenTradesCollectionViewSource = token.OpenTradesCollectionViewSource;
            }
            else if (token.EquityCollectionViewSource != null)
            {
                EquityCollectionViewSource = token.EquityCollectionViewSource;
            }
            else
            {
                AccountsCollectionViewSource = token.AccountSummaryCollectionViewSource;
            }
        }

        public  void Cleanup()
        {
            Messenger.Default.Unregister<ViewCollectionViewSourceMessageToken>(this);
            //base.Cleanup();
        }

        /// <summary>
        ///     Loads the data.
        /// </summary>
        private void LoadData()
        {
         
            LiveTrades = new ReactiveList<LiveTrade>(Context.LiveTrades.ToList());
            TradesHistory = new ReactiveList<TradeHistory>(Context.TradeHistories.ToList());
            OpenOrders = new ReactiveList<OpenOrder>(Context.OpenOrders.ToList());
            AccountSummaryCollection = new ReactiveList<PortfolioSummary>(Context.PortfolioSummaries.ToList());
            AccountsList = new ReactiveList<Account>(Context.Accounts.ToList());
            EquityCollection = new ReactiveList<Equity>(Context.Equities.ToList());
            Accounts = new ReactiveList<string>(AccountsList?.Select(x => x.AccountNumber));
            //SetUpModelData();

        }

        public ReactiveList<Account> AccountsList
        {
            get { return accountsList; }
            set { this.RaiseAndSetIfChanged(ref accountsList, value); }
        }

        /// <summary>
        ///     Updates the data.
        /// </summary>
        private void UpdateData()
        {
            //var livetrades = Context.GetLiveTrades();
            //var openorder = Context.GetOpenOrders();
            //var summary = Context.GetPortfolioSummary();
            var tradeHistory = messageHandler.UpdateTradeHistory(TradesHistory.Count-1);
            if (tradeHistory?.Count > 0)
            {
                Context.TradeHistories.AddRange(tradeHistory);
                Context.SaveChangesAsync();

                foreach (TradeHistory tradeHistor in tradeHistory)
                {
                    Application.Current.Dispatcher.Invoke(() => TradesHistory.Add(tradeHistor));
                }
            }

            LiveTrades = new ReactiveList<LiveTrade>(messageHandler.UpdateLiveTrades(LiveTrades.ToList()));

            OpenOrders = new ReactiveList<OpenOrder>(Context.OpenOrders.ToList());

            AccountSummaryCollection = new ReactiveList<PortfolioSummary>(Context.PortfolioSummaries.ToList());
        }

 



        /// <summary>
        ///     Updates the equity data.
        /// </summary>
        private void UpdateEquityData()
        {
            List<Equity> equity;
            if (EquityCollection.Count > 0)
            {
                var lastIdEquity = EquityCollection[EquityCollection.Count - 1].ID;
                equity = Context.Equities.Where(x=>x.ID>lastIdEquity).ToList();
            }
            else
            {
                equity = Context.Equities.ToList();
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
            var listEquity = EquityCollectionViewSource.View.Cast<Equity>().ToList();
            if (listEquity.Count != 0)
            {
                var dataperaccount =
                    listEquity.DistinctBy(x => x.ID).GroupBy(m => m.Account).OrderBy(m => m.Key).ToList();

                var min = Convert.ToDouble(listEquity.MinBy(m => m.Value).Value);
                var max = Convert.ToDouble(listEquity.MaxBy(m => m.Value).Value);

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

                    data.ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.UpdateTime), (double) d.Value)));
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
            ReloadDataCommand = ReactiveCommand.Create(LoadData);

        }
        public ReactiveCommand<Unit, Unit> ReloadDataCommand { get; set; }

        private void timer_tick_equity(object sender, EventArgs e)
        {
            UpdateData();
        }

        private async void timer_tick(object sender, EventArgs e)
        {
            await ReloadDataCommand.Execute();
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
                else if (string.Compare(SelectedAccount, src.Account.AccountNumber) != 0)
                    e.Accepted = false;
            }
            else if (e.Item is OpenOrder)
            {
                var src = (OpenOrder) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account.AccountNumber) != 0)
                    e.Accepted = false;
            }
            else if (e.Item is TradeHistory)
            {
                var src = (TradeHistory) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account.AccountNumber) != 0)
                    e.Accepted = false;
            }
            else if (e.Item is Equity)
            {
                var src = (Equity) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account.AccountNumber) != 0)
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
                else if (string.Compare(SelectedAccount, src.Account.AccountNumber) != 0)
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
                TradesHistoryCollectionViewSource.Filter -= FilterByAccount;
                TradesHistoryCollectionViewSource.Filter += FilterByAccount;
                LiveTradesCollectionViewSource.Filter -= FilterByAccount;
                LiveTradesCollectionViewSource.Filter += FilterByAccount;
                OpenTradesCollectionViewSource.Filter -= FilterByAccount;
                OpenTradesCollectionViewSource.Filter += FilterByAccount;
                AccountsCollectionViewSource.Filter -= FilterByAccount;
                AccountsCollectionViewSource.Filter += FilterByAccount;
                EquityCollectionViewSource.Filter += FilterByAccount;
                EquityCollectionViewSource.Filter -= FilterByAccount;
            }
            else
            {
                TradesHistoryCollectionViewSource.Filter += FilterByAccount;
                OpenTradesCollectionViewSource.Filter += FilterByAccount;
                LiveTradesCollectionViewSource.Filter += FilterByAccount;
                AccountsCollectionViewSource.Filter += FilterByAccount;
                EquityCollectionViewSource.Filter += FilterByAccount;
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
                TradesHistoryCollectionViewSource.Filter -= FilterByEndDate;
                TradesHistoryCollectionViewSource.Filter += FilterByEndDate;
                EquityCollectionViewSource.Filter -= FilterByEndDate;
                EquityCollectionViewSource.Filter += FilterByEndDate;
            }
            else
            {
                TradesHistoryCollectionViewSource.Filter += FilterByEndDate;
                EquityCollectionViewSource.Filter += FilterByEndDate;
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
                TradesHistoryCollectionViewSource.Filter -= FilterByStartDate;
                EquityCollectionViewSource.Filter -= FilterByStartDate;
                TradesHistoryCollectionViewSource.Filter += FilterByStartDate;
                EquityCollectionViewSource.Filter += FilterByStartDate;
            }
            else
            {
                TradesHistoryCollectionViewSource.Filter += FilterByStartDate;
                EquityCollectionViewSource.Filter += FilterByStartDate;
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
                LiveTradesCollectionViewSource.Filter -= FilterByAccount;
                OpenTradesCollectionViewSource.Filter -= FilterByAccount;
                TradesHistoryCollectionViewSource.Filter -= FilterByAccount;
                AccountsCollectionViewSource.Filter -= FilterByAccount;
                EquityCollectionViewSource.Filter -= FilterByAccount;

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
                TradesHistoryCollectionViewSource.Filter -= FilterByEndDate;
                EquityCollectionViewSource.Filter -= FilterByEndDate;
                //SelectedEndDate = new DateTime(2020, 1, 1);
            }
            if (CanRemoveStartDateFilter)
            {
                TradesHistoryCollectionViewSource.Filter -= FilterByStartDate;
                EquityCollectionViewSource.Filter -= FilterByStartDate;
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