using System;
using System.Linq;

using EntityData;
using MoreLinq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using QDMS;
using ReactiveUI;

namespace OverviewApp.ViewModels
{
    public class EquityViewModel : MyBaseViewModel
    {
        #region Fields

        
        private ReactiveList<Equity> equityCollection;
        private DateTime lastUpdate = DateTime.Now;
        private PlotModel plotModel;

        #endregion

        #region

        public EquityViewModel(IMyDbContext context) : base(context)
        {
            PlotModel = new PlotModel();
            SetUpModel();
         

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
                this.RaiseAndSetIfChanged(ref plotModel, value);
            }
        }

        public ReactiveList<Equity> EquityCollection
        {
            get { return equityCollection; }
            set { this.RaiseAndSetIfChanged(ref equityCollection, value); }
        }

        #endregion

        private void LoadData()
        {
            EquityCollection = new ReactiveList<Equity>(Context.Equity.ToList());
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
                    Maximum = (double) max,
                    Minimum = (double) min,
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
                        .ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.UpdateTime), (double) d.Value)));
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