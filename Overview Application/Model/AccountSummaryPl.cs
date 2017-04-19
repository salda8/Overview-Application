using System.ComponentModel;

namespace OverviewApp.Model
{
    public class AccountSummaryPl
    {
        public int ID { get; set; }

        [DisplayName("Net Liquidation")]
        public decimal NetLiquidation { get; set; }

        [DisplayName("Cash Balance")]
        public decimal CashBalance { get; set; }

        [DisplayName("Unrealized PnL")]
        public decimal UnrealizedPnL { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }
    }
}