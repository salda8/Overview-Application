using System;
using QDMS;

namespace OverviewApp.TradingEntitiesPl
{

    public class LiveTradePl : IEntity
    {
        #region Properties
       
        public int ID { get; set; }
       
        public virtual Instrument Instrument { get; set; }
        
        public int InstrumentID { get; set; }
      
        public decimal Quantity { get; set; }
       
        public TradeDirection TradeDirection { get; set; }
      
        public decimal MarketPrice { get; set; }
       
        public decimal MarketValue { get; set; }
        
        public decimal AveragePrice { get; set; }
      
        public decimal UnrealizedPnL { get; set; }
      
        public decimal RealizedPnl { get; set; }
      
        public virtual Account Account { get; set; }
     
        public int AccountID { get; set; }
       
        public virtual DateTime UpdateTime { get; set; }
       
        public int Port { get; set; }

        #endregion
    }
}