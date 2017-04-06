using System;
using QDMS;

namespace OverviewApp.TradingEntitiesPl { 

    public class EquityPl
    {
        #region Properties

       public int ID { get; set; }

        public virtual Account Account { get; set; }

        public int AccountID { get; set; }

        public decimal Value { get; set; }

        public DateTime UpdateTime { get; set; }

        #endregion
    }
}