using System;

namespace DataStructures.POCO
{
    public class PortfolioSummary
    {
        #region Properties

        public string Account { get; set; }
        public string StrategyName { get; set; }
        public double NetLiquidation { get; set; }
        public bool LivePosition { get; set; }
        public double OpenPnl { get; set; }
        public double StartEquity { get; set; }
        public DateTime StartDate { get; set; }
        public double Profit { get; set; }
        public double ProfitPercent { get; set; }
        public int DaysRunning { get; set; }
        public double DailyPercent { get; set; }
        public int GatewayPort { get; set; }

        #endregion
    }
}