using System;
using System.ComponentModel;
using Common.Enums;
using Common.Interfaces;

namespace OverviewApp.Model
{
    public class TradeHistoryPl : IEntity
    {
        public int ID { get; set; }

        //[DisplayName("Execution ID")]
        public string ExecutionID { get; set; }

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
        public DateTime ExecutionTime { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }
    }
}