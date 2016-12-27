using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using DataAccess;
using DataStructures;
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
    public class BarsViewModel : MyBaseViewModel
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly ILogger logger;
        private readonly Timer timer;

        private ObservableCollection<Candlestick> barsCollection;

        private bool canRemoveSymbolFilter;

        private bool canRemoveTimeframeFilter;

        private ObservableCollection<TradeHistory> historytradesCollection;

        private ObservableCollection<LiveTrade> livetradesCollection;

        private string selectedSymbol;

        private string selectedTimeframe;

        private ObservableCollection<string> symbol;
        private ObservableCollection<int> timeframe;
        private DateTime lastUpdate = DateTime.Now;
        private CandleStickSeries lineSerie;
        // private List<Candlestick> FilteredBars = new List<Candlestick>();

        private PlotModel plotModel;

        #endregion

        #region

        //private List<Candlestick> FilteredBarsBySymbol = new List<Candlestick>();
        // private List<Candlestick> FilteredBarsByTimeframe = new List<Candlestick>();

        public BarsViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            PlotModel = new PlotModel();
            _dataService = dataService;
            this.logger = logger;
            LoadData();

            SetUpModel();
            InitializeCommands();
            timer = new Timer();
            timer.Elapsed += tick_handler;
            timer.Interval = 60000;
            timer.Enabled = true;

            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        private CollectionViewSource Bcvs { get; set; }

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                RaisePropertyChanged();
            }
        }

        public bool CanRemoveTimeframeFilter
        {
            get { return canRemoveTimeframeFilter; }
            set
            {
                canRemoveTimeframeFilter = value;
                //RaisePropertyChanged("CanRemoveAuthorFilter");
                RaisePropertyChanged("CanRemoveTimeframeFilter");
            }
        }

        public bool CanRemoveSymbolFilter
        {
            get { return canRemoveSymbolFilter; }
            set
            {
                canRemoveSymbolFilter = value;
                //RaisePropertyChanged("CanRemoveAuthorFilter");
                RaisePropertyChanged("CanRemovSymbolFilter");
            }
        }

        public ObservableCollection<Candlestick> BarsCollection
        {
            get { return barsCollection; }
            set
            {
                if (Equals(value, barsCollection)) return;
                barsCollection = value;
                RaisePropertyChanged();
            }
        }

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

        /// <summary>
        ///     Gets or sets a list of timeframe which is used to populate the account filter
        ///     drop down list.
        /// </summary>
        public ObservableCollection<int> Timeframe
        {
            get { return timeframe; }
            set
            {
                if (timeframe == value)
                    return;
                timeframe = value;
                //RaisePropertyChanged("Authors");
                RaisePropertyChanged();
                //UpdateModel();
            }
        }

        public ObservableCollection<string> Symbol
        {
            get { return symbol; }
            set
            {
                if (symbol == value) return;
                symbol = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedSymbol
        {
            get { return selectedSymbol; }
            set
            {
                if (selectedSymbol == value) return;
                selectedSymbol = value;
                RaisePropertyChanged();
                ApplyFilter(!string.IsNullOrEmpty(selectedSymbol) ? FilterField.Symbol : FilterField.None);
                UpdateModel();
            }
        }

        /// <summary>
        ///     Gets or sets the selected account in the list to filter the collection
        /// </summary>
        public string SelectedTimeframe
        {
            get { return selectedTimeframe; }
            set
            {
                if (selectedTimeframe == value)
                    return;
                selectedTimeframe = value;
                RaisePropertyChanged();
                //RaisePropertyChanged();

                ApplyFilter(!string.IsNullOrEmpty(selectedTimeframe) ? FilterField.Timeframe : FilterField.None);

                UpdateModel();
            }
        }

        public RelayCommand ResetFiltersCommand { get; private set; }
        public RelayCommand RemoveTimeframeFilterCommand { get; private set; }

        #endregion

        #region Nested

        private enum FilterField
        {
            Symbol,
            Timeframe,

            None
        }

        #endregion

        private void tick_handler(object sender, ElapsedEventArgs e)
        {
            UpdateBarsData();
        }

        private void UpdateBarsData()
        {
            if (BarsCollection.Count > 0)
            {
                var lastId = BarsCollection.Count - 1;
                var lastBarId = BarsCollection[lastId].Id;
                var newbars = _dataService.GetBars(lastBarId);
                if (newbars.Count > 0)
                {
                    foreach (var bars in newbars)
                    {
                        Application.Current.Dispatcher.Invoke(() => { BarsCollection.Add(bars); });

                        if (bars.Interval == Convert.ToInt16(selectedTimeframe) && bars.Symbol == selectedSymbol)
                        {
                            lineSerie.Items.Add(new HighLowItem(DateTimeAxis.ToDouble(bars.BarTime), bars.High, bars.Low,
                                bars.Open, bars.Close));
                            plotModel.InvalidatePlot(true);
                        }
                    }
                }
            }
        }

        private void ResetFilters()
        {
            RemoveBarsFilter();
            //_selectedTimeframe = "";
            //_selectedSymbol = "";
            SelectedTimeframe = "";
            SelectedSymbol = "";
            SetUpModel();
        }

        private void RemoveBarsFilter()
        {
            Bcvs.Filter -= FilterByTimeframe;
        }

        private void InitializeCommands()
        {
            RemoveTimeframeFilterCommand = new RelayCommand(RemoveTimeframeFilter, () => CanRemoveTimeframeFilter);
            ResetFiltersCommand = new RelayCommand(ResetFilters, null);
        }

        private void RemoveTimeframeFilter()
        {
            throw new NotImplementedException();
        }

        private void LoadData()
        {
            BarsCollection = _dataService.GetBars();
            LiveTrades = _dataService.GetLiveTrades();
            TradesHistory = _dataService.GetTradeHistory();
            var tf = from t in BarsCollection
                select t.Interval;
            Timeframe = new ObservableCollection<int>(tf.Distinct());
            var symbol = from s in BarsCollection
                select s.Symbol;
            Symbol = new ObservableCollection<string>(symbol.Distinct());

            lastUpdate = DateTime.Now;
        }

        private void SetUpModel()
        {
            PlotModel.Series.Clear();
            PlotModel.Axes.Clear();
            //FilteredBars.Clear();
            //FilteredBarsBySymbol.Clear();
            //FilteredBarsByTimeframe.Clear();

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
                IntervalLength = 80
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
            PlotModel.InvalidatePlot(true);
        }

        public void UpdateModel()
        {
            var filteredBarsList = Bcvs.View.OfType<Candlestick>().ToList();

            if (filteredBarsList.Count != 0)
            {
                var min = filteredBarsList.MinBy(m => m.Low).Low;
                var max = filteredBarsList.MaxBy(m => m.High).High;

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
                lineSerie = new CandleStickSeries
                {
                    StrokeThickness = 1,
                    TrackerFormatString =
                        "Date: {2:HH.mm dd.MM.yy}" + Environment.NewLine + "Open: {5}" + Environment.NewLine +
                        "High: {3}" + Environment.NewLine + "Low: {4}" + Environment.NewLine + "Close: {6}",
                    Title =
                        $"Symbol {filteredBarsList[0].Symbol + ":" + filteredBarsList[0].Interval + " minute"}"
                };

                foreach (var data in filteredBarsList)
                {
                    lineSerie.Items.Add(new HighLowItem(DateTimeAxis.ToDouble(data.BarTime), data.High, data.Low,
                        data.Open, data.Close));
                }
                ;
                PlotModel.Series.Add(lineSerie);
                PlotModel.InvalidatePlot(true);
            }
        }

        private void AddSymbolFilter()
        {
            if (CanRemoveTimeframeFilter)
            {
                Bcvs.Filter -= FilterBySymbol;
                Bcvs.Filter += FilterBySymbol;
            }
            else
            {
                Bcvs.Filter += FilterBySymbol;

                CanRemoveSymbolFilter = true;
            }
        }

        private void FilterBySymbol(object sender, FilterEventArgs e)
        {
            if (e.Item is Candlestick)
            {
                var src = (Candlestick) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedSymbol, src.Symbol) != 0)
                {
                    e.Accepted = false;
                }
            }
        }

        private void AddTimeframeFilter()
        {
            if (CanRemoveTimeframeFilter)
            {
                Bcvs.Filter -= FilterByTimeframe;
                Bcvs.Filter += FilterByTimeframe;
            }
            else
            {
                Bcvs.Filter += FilterByTimeframe;

                CanRemoveTimeframeFilter = true;
            }
        }

        private void FilterByTimeframe(object sender, FilterEventArgs e)
        {
            if (e.Item is Candlestick)
            {
                var src = (Candlestick) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedTimeframe, Convert.ToString(src.Interval)) != 0)
                {
                    e.Accepted = false;
                }
            }
        }

        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken token)
        {
            if (token.BarsCollectionViewSource != null)
            {
                Bcvs = token.BarsCollectionViewSource;
            }
        }

        private void ApplyFilter(FilterField field)
        {
            switch (field)
            {
                case FilterField.Timeframe:
                    //FilteredBars.Clear();
                    AddTimeframeFilter();
                    break;

                case FilterField.Symbol:
                    //FilteredBars.Clear();
                    AddSymbolFilter();
                    //AddCountryFilter();
                    break;

                default:
                    break;
            }
        }
    }
}