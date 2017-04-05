using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using EntityData;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MoreLinq;
using OverviewApp.Auxiliary.Helpers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ReactiveUI;
using QDMS;


namespace OverviewApp.ViewModels
{
    public class BarsViewModel : MyBaseViewModel
    {
        #region Fields

        private readonly Timer timer;

        private ObservableCollection<OHLCBar> barsCollection= new ObservableCollection<OHLCBar>();

        private bool canRemoveSymbolFilter;

        private bool canRemoveTimeframeFilter;

        private ObservableCollection<TradeHistory> historytradesCollection;

        private ObservableCollection<LiveTrade> livetradesCollection;

        private int selectedInstrumentId;

        private BarSize selectedTimeframe;

        private ObservableCollection<Instrument> instruments;
        private ObservableCollection<BarSize> timeframe;
        private CandleStickSeries lineSerie;
        // private List<Candlestick> FilteredBars = new List<Candlestick>();

        private PlotModel plotModel;

        public DataDBContext DataDBContext { get; set; }

        #endregion

        #region

        //private List<Candlestick> FilteredBarsBySymbol = new List<Candlestick>();
        // private List<Candlestick> FilteredBarsByTimeframe = new List<Candlestick>();

        public BarsViewModel(IMyDbContext context, DataDBContext dataDbContext) : base(context)
        {
            PlotModel = new PlotModel();
            DataDBContext = dataDbContext;
            
            Instruments = new ObservableCollection<Instrument>(Context.Instruments.ToList());
            Timeframe = new ObservableCollection<BarSize>() { BarSize.FifteenMinutes, BarSize.FifteenSeconds, BarSize.FiveMinutes, BarSize.FiveSeconds, BarSize.OneDay,
                BarSize.OneHour, BarSize.OneMinute, BarSize.OneMonth, BarSize.OneQuarter, BarSize.OneSecond, BarSize.OneWeek, BarSize.OneYear, BarSize.ThirtyMinutes, BarSize.ThirtySeconds, BarSize.Tick };
            LoadData();
            
            SetUpModel();
            InitializeCommands();
            timer = new Timer();
            timer.Elapsed += tick_handler;
            timer.Interval = 60000;
            timer.Enabled = true;
           
            Logger.Info(() => "BarsViewModel init");

            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        private CollectionViewSource Bcvs { get; set; }

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { this.RaiseAndSetIfChanged(ref plotModel, value); }
        }

        public bool CanRemoveTimeframeFilter
        {
            get { return canRemoveTimeframeFilter; }
            set
            {
               
                this.RaiseAndSetIfChanged(ref canRemoveTimeframeFilter, value);
            }
        }

        public bool CanRemoveSymbolFilter
        {
            get { return canRemoveSymbolFilter; }
            set
            {
               
                this.RaiseAndSetIfChanged(ref canRemoveSymbolFilter, value);
            }
        }

        public ObservableCollection<OHLCBar> BarsCollection
        {
            get { return barsCollection; }
            set { this.RaiseAndSetIfChanged(ref barsCollection, value); }
        }

        public ObservableCollection<LiveTrade> LiveTrades
        {
            get { return livetradesCollection; }
            set { this.RaiseAndSetIfChanged(ref livetradesCollection, value); }
        }

        public ObservableCollection<TradeHistory> TradesHistory
        {
            get { return historytradesCollection; }
            set { this.RaiseAndSetIfChanged(ref historytradesCollection, value); }
        }

        /// <summary>
        ///     Gets or sets a list of timeframe which is used to populate the account filter
        ///     drop down list.
        /// </summary>
        public ObservableCollection<BarSize> Timeframe
        {
            get { return timeframe; }
            set { this.RaiseAndSetIfChanged(ref timeframe, value); }
        }

        public ObservableCollection<Instrument> Instruments
        {
            get { return instruments; }
            set { this.RaiseAndSetIfChanged(ref instruments, value); }
        }

        public int SelectedInstrumentId
        {
            get { return selectedInstrumentId; }
            set
            {
                if (selectedInstrumentId == value) return;
                this.RaiseAndSetIfChanged(ref selectedInstrumentId, value);
                ApplyFilter(FilterField.Symbol);
                UpdateModel();
            }
        }

        /// <summary>
        ///     Gets or sets the selected account in the list to filter the collection
        /// </summary>
        public BarSize SelectedTimeframe
        {
            get { return selectedTimeframe; }
            set
            {
                if (selectedTimeframe == value)
                    return;
                this.RaiseAndSetIfChanged(ref selectedTimeframe, value);

                ApplyFilter( FilterField.Timeframe);

                UpdateModel();
            }
        }

        public RelayCommand ResetFiltersCommand { get; private set; }
        public RelayCommand RemoveTimeframeFilterCommand { get; private set; }

        #endregion

        #region Nested

        

        #endregion

        private void tick_handler(object sender, ElapsedEventArgs e)
        {
            UpdateBarsData();
        }

        private void UpdateBarsData()
        {
            if (BarsCollection!=null && BarsCollection.Count > 0)
            {
                DateTime? lastBarId = BarsCollection.LastOrDefault()?.DT;
                List<OHLCBar> newbars;
                if (lastBarId == null)
                {
                    newbars =
                        DataDBContext.Data.Where(
                            x => x.Frequency == selectedTimeframe && x.InstrumentID == selectedInstrumentId).ToList();

                }
                else
                {
                    newbars =
                        DataDBContext.Data.Where(
                            x =>
                                x.DT > lastBarId && x.Frequency == selectedTimeframe &&
                                x.InstrumentID == selectedInstrumentId).ToList();
                }
                if (newbars.Any())
                {
                    foreach (var bars in newbars)
                    {
                        Application.Current.Dispatcher.Invoke(() => { BarsCollection.Add(bars); });

                       
                            lineSerie.Items.Add(new HighLowItem(DateTimeAxis.ToDouble(bars.DT), (double) bars.High, (double) bars.Low,
                                (double) bars.Open, (double) bars.Close));
                            plotModel.InvalidatePlot(true);
                        
                    }
                }
            }
        }

        private void ResetFilters()
        {
            RemoveBarsFilter();
            //_selectedTimeframe = "";
            //_selectedSymbol = "";
            SelectedTimeframe = BarSize.OneHour;
            SelectedInstrumentId = 0;
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
            var bars = DataDBContext.Data.Where(x=>selectedInstrumentId==x.InstrumentID && SelectedTimeframe==x.Frequency);
            foreach (var bar in bars)
            {
                BarsCollection.Add(bar);
            }

            
            
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
                        $"Instruments {filteredBarsList[0].Instrument.Symbol + ":" + filteredBarsList[0].Interval + " minute"}"
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
            if (e.Item is OHLCBar)
            {
                var src = (OHLCBar) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (SelectedInstrumentId == src.InstrumentID)
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
            if (e.Item is OHLCBar)
            {
                var src = (OHLCBar) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (SelectedTimeframe== src.Frequency)
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