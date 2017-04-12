using Common;
using System;
using System.ComponentModel;
using Common.Interfaces;

namespace OverviewApp.TradingEntitiesPl
{
    public class OpenOrderPl : IEntity
    {
        #region Properties

        public int ID { get; set; }

        [DisplayName("Permanent ID")]
        public int PermanentId { get; set; }

        [DisplayName("Symbol")]
        public string Symbol { get; set; }

        [DisplayName("Instrument ID")]
        public int InstrumentID { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Limit Price")]
        public decimal LimitPrice { get; set; }

        [DisplayName("Quantity")]
        public decimal Quantity { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }

        [DisplayName("Update Time")]
        public DateTime UpdateTime { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        #endregion Properties
    }
}