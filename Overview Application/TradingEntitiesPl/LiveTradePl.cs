using Common;
using System;
using System.ComponentModel;

namespace OverviewApp.TradingEntitiesPl
{
    public class LiveTradePl : IEntity
    {
        public int ID { get; set; }

        [DisplayName("Symbol")]
        public string Symbol { get; set; }

        public int InstrumentID { get; set; }

        [DisplayName("Quantity")]
        public decimal Quantity { get; set; }

        [DisplayName("Trade Direction")]
        public TradeDirection TradeDirection { get; set; }

        [DisplayName("Market Price")]
        public decimal MarketPrice { get; set; }

        [DisplayName("Market Value")]
        public decimal MarketValue { get; set; }

        [DisplayName("Average Price")]
        public decimal AveragePrice { get; set; }

        [DisplayName("Unrealized PnL")]
        public decimal UnrealizedPnL { get; set; }

        [DisplayName("Realized PnL")]
        public decimal RealizedPnl { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }

        [DisplayName("Update Time")]
        public virtual DateTime UpdateTime { get; set; }
    }
}