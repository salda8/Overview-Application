using Common;
using System;
using System.ComponentModel;

namespace OverviewApp.TradingEntitiesPl
{
    public class TradeHistoryPl : IEntity
    {
        public int ID { get; set; }

        //[DisplayName("Execution ID")]
        public string ExecId { get; set; }

        [DisplayName("Quantity")]
        public decimal Quantity { get; set; }

        [DisplayName("Side")]
        public TradeDirection Side { get; set; }

        [DisplayName("Symbol")]
        public string Symbol { get; set; }

        public int InstrumentID { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Commission")]
        public decimal Commission { get; set; }

        [DisplayName("Realized PnL")]
        public decimal RealizedPnL { get; set; }

        [DisplayName("Execution Time")]
        public DateTime ExecTime { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }
    }
}