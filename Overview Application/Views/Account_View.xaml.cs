using System.Windows.Controls;
using System.Windows.Data;
using EntityData;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.ViewModels;
using ReactiveUI;

namespace OverviewApp.Views
{
    /// <summary>
    ///     Interaction logic for Account_View.xaml
    /// </summary>
    public partial class AccountView : UserControl, IViewFor<AccountViewModel>
    {
        #region

        public AccountView()
        {
            var myDbContext = ServiceLocator.Current.GetInstance<IMyDbContext>();
            ViewModel = new AccountViewModel(myDbContext);
            DataContext = ViewModel;
            InitializeComponent();
            //Messenger.Default.Send(new ViewCollectionViewSourceMessageToken() { LiveTradesCollectionViewSource = (CollectionViewSource)(Resources["X_CVS"]), TradesHistoryCollectionViewSource = (CollectionViewSource)(Resources["T_CVS"]), OpenTradesCollectionViewSource = (CollectionViewSource)(Resources["O_CVS"]), AccountSummaryCollectionViewSource = (CollectionViewSource)(Resources["S_CVS"]) });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                LiveTradesCollectionViewSource = (CollectionViewSource)Resources["LiveTradesCvs"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                TradesHistoryCollectionViewSource = (CollectionViewSource)Resources["TradeHistoryCvs"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                OpenTradesCollectionViewSource = (CollectionViewSource)Resources["OpenOrdersCvs"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                AccountSummaryCollectionViewSource = (CollectionViewSource)Resources["AccountsCvs"]
            });
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                EquityCollectionViewSource = (CollectionViewSource)Resources["EquityCvs"]
            });
        }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AccountViewModel) value; }
        }

        public AccountViewModel ViewModel { get; set; }

        #endregion
    }
}