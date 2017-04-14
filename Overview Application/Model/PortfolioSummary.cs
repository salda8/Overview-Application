using System;
using System.ComponentModel;
using Common.EntityModels;
using Common.Interfaces;

namespace OverviewApp.Model
{
    public class PortfolioSummary1 : IEntity
    {
        #region Properties

        public int ID { get; set; }
        public virtual Account Account { get; set; }
        public int AccountID { get; set; }

        public decimal NetLiquidation { get; set; }
        public bool LivePosition { get; set; }
        public decimal OpenPnl { get; set; }
        public decimal StartEquity { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPercent { get; set; }
        public int DaysRunning { get; set; }
        public decimal DailyPercent { get; set; }
        public int GatewayPort { get; set; }
        public virtual Strategy Strategy { get; set; }
        public int StrategyID { get; set; }

        #endregion Properties
    }

    public class PortfolioSummaryPl : IEntity
    {
        #region Properties

        public int ID { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }

        public string Symbol { get; set; }

        public int StrategyID { get; set; }

        public decimal Quantity { get; set; }

        public decimal MarketValue { get; set; }

        public decimal MarketPrice { get; set; }

        public decimal AverageCost { get; set; }

        public decimal UnrealizedPnL { get; set; }

        public decimal RealizedPnL { get; set; }

        //public decimal NetLiquidation { get; set; }
        //public bool LivePosition { get; set; }
        //public decimal OpenPnl { get; set; }
        //public decimal StartEquity { get; set; }
        //public DateTime StartDate { get; set; }
        //public decimal Profit { get; set; }
        //public decimal ProfitPercent { get; set; }
        //public int DaysRunning { get; set; }
        //public decimal DailyPercent { get; set; }
        //public int GatewayPort { get; set; }

        #endregion Properties
    }
}