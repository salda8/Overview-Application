using System.Windows.Data;

namespace OverviewApp.Auxiliary.Helpers
{
    public class ViewCollectionViewSourceMessageToken
    {
        #region Properties

        public CollectionViewSource LiveTradesCollectionViewSource { get; set; }
        public CollectionViewSource TradesHistoryCollectionViewSource { get; set; }
        public CollectionViewSource OpenTradesCollectionViewSource { get; set; }
        public CollectionViewSource AccountSummaryCollectionViewSource { get; set; }
        public CollectionViewSource EquityCollectionViewSource { get; set; }
        public CollectionViewSource BarsCollectionViewSource { get; set; }
        public CollectionViewSource MatlabValuesCollectionViewSource { get; set; }
        public CollectionViewSource StrategyCollectionViewSource { get; set; }

        #endregion
    }
}