using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;

namespace OverviewApp.Views
{
    /// <summary>
    ///     Interaction logic for Account_View.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        #region

        public AccountView()
        {
            InitializeComponent();
            //Messenger.Default.Send(new ViewCollectionViewSourceMessageToken() { LiveTradesCollectionViewSource = (CollectionViewSource)(Resources["X_CVS"]), TradesHistoryCollectionViewSource = (CollectionViewSource)(Resources["T_CVS"]), OpenTradesCollectionViewSource = (CollectionViewSource)(Resources["O_CVS"]), AccountSummaryCollectionViewSource = (CollectionViewSource)(Resources["S_CVS"])});
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                LiveTradesCollectionViewSource = (CollectionViewSource) Resources["C_CVS"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                TradesHistoryCollectionViewSource = (CollectionViewSource) Resources["T_CVS"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                OpenTradesCollectionViewSource = (CollectionViewSource) Resources["O_CVS"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                AccountSummaryCollectionViewSource = (CollectionViewSource) Resources["S_CVS"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                EquityCollectionViewSource = (CollectionViewSource) Resources["E_CVS"]
            });
        }

        #endregion
    }
}