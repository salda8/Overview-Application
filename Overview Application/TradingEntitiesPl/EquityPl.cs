using System;
using System.ComponentModel;
using QDMS;

namespace OverviewApp.TradingEntitiesPl { 

    public class EquityPl
    {
       

       public int ID { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }

        public decimal Value { get; set; }

        public DateTime UpdateTime { get; set; }

       
    }
}