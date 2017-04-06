using System;
using System.ComponentModel;
using QDMS;

namespace OverviewApp.TradingEntitiesPl
{

    public class LiveTradePl : IEntity
    {
       
       
        public int ID { get; set; }

        public string Symbol { get; set; }

        public int InstrumentID { get; set; }
      
        public decimal Quantity { get; set; }
       
        public TradeDirection TradeDirection { get; set; }
      
        public decimal MarketPrice { get; set; }
       
        public decimal MarketValue { get; set; }
        
        public decimal AveragePrice { get; set; }
      
        public decimal UnrealizedPnL { get; set; }
      
        public decimal RealizedPnl { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }
       
        public virtual DateTime UpdateTime { get; set; }
       
        public int Port { get; set; }

    }
}