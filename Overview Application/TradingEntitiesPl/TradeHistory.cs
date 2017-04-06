using System;
using QDMS;

namespace OverviewApp.TradingEntitiesPl
{
    
    public class TradeHistoryPl : IEntity
    {
        public int ID { get; set; }
        public string ExecId { get; set; }
        public decimal Quantity { get; set; }
        public TradeDirection Side { get; set; }
        public virtual Instrument Instrument { get; set; }
        public int InstrumentID { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }
        public decimal RealizedPnL { get; set; }
        public DateTime ExecTime { get; set; }
        public virtual Account Account { get; set; }
        public int AccountID { get; set; }
    }
}