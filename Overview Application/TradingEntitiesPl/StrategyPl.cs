using Common;

namespace OverviewApp.TradingEntitiesPl
{
    public class StrategyPl : IEntity
    {
        public int ID { get; set; }

        public string StrategyName { get; set; }

        public int ParamId { get; set; }

        public decimal CalmariRatio { get; set; }

        public virtual Instrument Instrument { get; set; }

        public int InstrumentID { get; set; }

        public decimal BacktestPeriod { get; set; }

        public decimal BacktestDrawDown { get; set; }

        public decimal BacktestProfit { get; set; }

        public string Filepath { get; set; }
    }
}