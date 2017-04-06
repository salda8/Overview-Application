using System;
using System.ComponentModel;
using QDMS;

namespace OverviewApp.TradingEntitiesPl
{
   
    public class OpenOrderPl : IEntity
    {
        #region Properties
        
        public int ID { get; set; }
      
        public int PermanentId { get; set; }

        public string Symbol { get; set; }

        public int InstrumentID { get; set; }
       
        public string Status { get; set; }
    
        public decimal LimitPrice { get; set; }
       
        public decimal Quantity { get; set; }
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }
      
        public DateTime UpdateTime { get; set; }
       
        public string Type { get; set; }

        #endregion
    }
}