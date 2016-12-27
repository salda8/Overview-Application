using System;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess;
using DataStructures;
using DataStructures.POCO;
using MoreLinq;
using OverviewApp.Logger;
using OverviewApp.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace OverviewApp.ViewModels
{
    public class EquityViewModel : MyBaseViewModel
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly ILogger logger;
        private ObservableCollection<Equity> equityCollection;
        private DateTime lastUpdate = DateTime.Now;
        private PlotModel plotModel;

        #endregion

        #region

        public EquityViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            PlotModel = new PlotModel();
            SetUpModel();
            _dataService = dataService;
            this.logger = logger;

            LoadData();
        }

        #endregion

        #region Properties

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
                RaisePropertyChanged();
            }
        }

        #endregion

        private void LoadData()
        {
            EquityCollection = _dataService.GetEquity();
            var dataperaccount = EquityCollection.GroupBy(m => m.Account).OrderBy(m => m.Key).ToList();
            if (EquityCollection.Count > 0)
            {
                var min = EquityCollection.MinBy(m => m.Value).Value;
                var max = EquityCollection.MaxBy(m => m.Value).Value;
#pragma warning disable CS0612 // 'LinearAxis.LinearAxis(AxisPosition, double, double, string)' is obsolete
                var valueAxis = new LinearAxis(AxisPosition.Left, 0)
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    Maximum = max,
                    Minimum = min,
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
                        CanTrackerInterpolatePoints = false,
                        Title = $"Account {data.Key}",
                        Smooth = false
                    };

                    data.ToList()
                        .ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.UpdateTime), d.Value)));
                    PlotModel.Series.Add(lineSerie);
                }
            }
            lastUpdate = DateTime.Now;
            PlotModel.InvalidatePlot(true);
        }

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
                IntervalLength = 80
            };
#pragma warning restore CS0612 // 'DateTimeAxis.DateTimeAxis(AxisPosition, string, string, DateTimeIntervalType)' is obsolete
            PlotModel.Axes.Add(dateAxis);
        }

        public void UpdateModel()
        {
        }
    }
}