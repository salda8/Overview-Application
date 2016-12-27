namespace DataStructures.POCO
{
    public class Strategy
    {
        #region Properties

        public string StrategyName { get; set; }
        public int ParamId { get; set; }
        public double Calmari { get; set; }
        public string Symbols { get; set; }
        public double BacktestPeriod { get; set; }
        public double BacktestDrawDown { get; set; }
        public double BacktestProfit { get; set; }
        public int Id { get; set; }
        public double DaysRunning { get; set; }
        public double Profit { get; set; }
        public double DailyProfit { get; set; }
        public double OpenPnL { get; set; }
        public string Filepath { get; set; }

        #endregion
    }
}