using System.ComponentModel;

namespace OverviewApp.TradingEntitiesPl
{
    public class AccountSummaryPl
    {
        public int ID { get; set; }

        [DisplayName("Net Liquidation")]
        public decimal NetLiquidation { get; set; }

        [DisplayName("Cash Balance")]
        public decimal CashBalance { get; set; }

        [DisplayName("Day Trades Remaining")]
        public decimal DayTradesRemaining { get; set; }

        [DisplayName("Equity With Loan Value")]
        public decimal EquityWithLoanValue { get; set; }

        [DisplayName("Init Margin Req")]
        public decimal InitMarginReq { get; set; }

        [DisplayName("Main Margin Req")]
        public decimal MaintMarginReq { get; set; }

        [DisplayName("Unrealized PnL")]
        public decimal UnrealizedPnL { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }
    }
}