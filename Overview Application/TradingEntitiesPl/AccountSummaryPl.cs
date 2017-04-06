namespace OverviewApp.TradingEntitiesPl
{
    
    public class AccountSummaryPl 
    {
       
        public int ID { get; set; }
       
        public decimal NetLiquidation { get; set; }
        
        public decimal CashBalance { get; set; }
   
        public decimal DayTradesRemaining { get; set; }

        public decimal EquityWithLoanValue { get; set; }
  
        public decimal InitMarginReq { get; set; }
    
        public decimal MaintMarginReq { get; set; }
   
        public decimal UnrealizedPnL { get; set; }
   
        public string AccountMame { get; set; }
       
    }
}
