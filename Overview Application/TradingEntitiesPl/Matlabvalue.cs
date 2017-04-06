using System;
using QDMS;

namespace OverviewApp.TradingEntitiesPl
{
   
    public class MatlabvaluePl : IEntity
    {
        #region Properties
      
        public int ID { get; set; }
      
        public virtual Instrument Instrument { get; set; }
      
        public int InstrumentID { get; set; }
      
        public decimal Value { get; set; }
      
        public DateTime Time { get; set; }
       
        public virtual Strategy Strategy { get; set; }
       
        public int StrategyID { get; set; }
        

        #endregion
    }
}