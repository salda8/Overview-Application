using System;
using QDMS;

namespace OverviewApp.TradingEntitiesPl
{
   
    public class OpenOrderPl : IEntity
    {
        #region Properties
        
        public int ID { get; set; }
      
        public int PermanentId { get; set; }
      
        public virtual Instrument Instrument { get; set; }
       
        public int InstrumentID { get; set; }
       
        public string Status { get; set; }
    
        public decimal LimitPrice { get; set; }
       
        public decimal Quantity { get; set; }
      
        public virtual Account Account { get; set; }
   
        public int AccountID { get; set; }
      
        public DateTime UpdateTime { get; set; }
       
        public string Type { get; set; }

        #endregion
    }
}