using QDMS;

namespace OverviewApp.TradingEntitiesPl
{
 
    public class AccountPl : IEntity
    {
       
        public int ID { get; set; }
      
        public string AccountNumber { get; set; }
       
        public string BrokerName { get; set; }
       
        public int Port { get; set; }
       
        public string IpAddress { get; set; }
        
        public decimal InitialBalance { get; set; }
        
        public virtual Strategy Strategy { get; set; }
       
        public int StrategyID { get; set; }

 
    }
}
