using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using QDMS;
using EntityData;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;

using ReactiveUI;
using Splat;


namespace OverviewApp.ViewModels
{
    public class CloseTradesViewModel : MyBaseViewModel
    {
     
        #region Fields

        private readonly List<Equity> filteredEquity = new List<Equity>();
        private readonly List<Equity> filteredEquityByDate = new List<Equity>();

        private ObservableCollection<string> accounts;

        private ObservableCollection<PortfolioSummary> accsummaryCollection;
        private bool canRemoveAccountFilter;
        private bool canRemoveEndDateFilter;
#pragma warning disable CS0169 // The field 'CloseTrades_ViewModel._canRemoveStartDateFilter' is never used
        private bool canRemoveStartDateFilter;
#pragma warning restore CS0169 // The field 'CloseTrades_ViewModel._canRemoveStartDateFilter' is never used

#pragma warning disable CS0169 // The field 'CloseTrades_ViewModel._equityCollection' is never used
        private ObservableCollection<Equity> equityCollection;
#pragma warning restore CS0169 // The field 'CloseTrades_ViewModel._equityCollection' is never used

        private ObservableCollection<LiveTrade> livetradesCollection;

        private ObservableCollection<OpenOrder> openordersCollection;

        /// <summary>
        ///     Gets or sets the selected account in the list to filter the collection
        /// </summary>
        private string selectedAccount;

        private DateTime selectedEndDate = DateTime.Today;


        /// <summary>
        ///     Gets or sets the selected row.
        /// </summary>
        /// <value>
        ///     The selected row.
        /// </value>
        private LiveTrade selectedRow;

        private DateTime selectedStartDate = DateTime.Today.AddDays(-10);
#pragma warning disable CS0169 // The field 'CloseTrades_ViewModel.lastUpdateMilliSeconds' is never used
        private long lastUpdateMilliSeconds;
#pragma warning restore CS0169 // The field 'CloseTrades_ViewModel.lastUpdateMilliSeconds' is never used

        #endregion

        #region

        /// <summary>
        ///     Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        public CloseTradesViewModel(IMyDbContext context, ILogger logger) : base(context, logger)
        {
          
            InitializeCommands();
           
            LoadData();
            // This will register our method with the Messenger class for incoming
            // messages of type ViewCollectionViewSourceMessageToken.
            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the IDownloadContext member
        /// </summary>
       

        /// <summary>
        ///     Gets or sets the CollectionViewSource which is the proxy for the
        ///     collection of Things and the datagrid in which each thing is displayed.
        /// </summary>
        private CollectionViewSource Cvs { get; set; }

        public RelayCommand CloseTradeCommand { get; set; }
        public RelayCommand ResetFiltersCommand { get; private set; }
        public RelayCommand RemoveAccountFilterCommand { get; private set; }

        public ObservableCollection<LiveTrade> LiveTrades
        {
            get { return livetradesCollection; }
            set { this.RaiseAndSetIfChanged(ref livetradesCollection, value); }
        }

        public ObservableCollection<OpenOrder> OpenOrders
        {
            get { return openordersCollection; }
            set { this.RaiseAndSetIfChanged(ref openordersCollection, value); }
        }

        public ObservableCollection<PortfolioSummary> AccountSummaryCollection
        {
            get { return accsummaryCollection; }
            set { this.RaiseAndSetIfChanged(ref accsummaryCollection, value); }
        }

        /// <summary>
        ///     Gets or sets a list of accounts which is used to populate the account filter
        ///     drop down list.
        /// </summary>
        public ObservableCollection<string> Accounts
        {
            get { return accounts; }
            set { this.RaiseAndSetIfChanged(ref accounts, value); }
        }

        public string SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                if (selectedAccount == value)
                    return;
                this.RaiseAndSetIfChanged(ref selectedAccount, value);
                ApplyFilter(!string.IsNullOrEmpty(selectedAccount) ? FilterField.Account : FilterField.None);
            }
        }

        public LiveTrade SelectedRow
        {
            get { return selectedRow; }
            set { this.RaiseAndSetIfChanged(ref selectedRow, value); }
        }

        public bool CanRemoveAccountFilter
        {
            get { return canRemoveAccountFilter; }
            set { this.RaiseAndSetIfChanged(ref canRemoveAccountFilter, value); }
        }

        public bool CanRemoveEndDateFilter
        {
            get { return canRemoveEndDateFilter; }
            set { this.RaiseAndSetIfChanged(ref canRemoveEndDateFilter, value); }
        }

        public DateTime Today => DateTime.Today;

        #endregion

        #region Nested

        private enum FilterField
        {
            Account,
            None
        }

        #endregion

        /// <summary>
        ///     This method handles a message recieved from the View which enables a reference to the
        ///     instantiated CollectionViewSource to be used in the ViewModel.
        /// </summary>
        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken token)
        {
            if (token.LiveTradesCollectionViewSource != null)
            {
                Cvs = token.LiveTradesCollectionViewSource;
            }
        }

        public  void Cleanup()
        {
            Messenger.Default.Unregister<ViewCollectionViewSourceMessageToken>(this);
            //base.Cleanup();
        }

        /// <summary>
        ///     Loads the data.
        /// </summary>
        private void LoadData()
        {
            var livetrades = Context.LiveTrades.ToList();
            LiveTrades = new ObservableCollection<LiveTrade>(livetrades);
        }

        /// <summary>
        ///     Updates the data.
        /// </summary>
        private void UpdateData()
        {
            var livetrades = Context.LiveTrades.ToList();

            Application.Current.Dispatcher.Invoke(() => { LiveTrades.Clear(); });
            LiveTrades = new ObservableCollection<LiveTrade>(livetrades);
        }

        /// <summary>
        ///     Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
            RemoveAccountFilterCommand = new RelayCommand(RemoveAccountFilter, () => CanRemoveAccountFilter);
            ResetFiltersCommand = new RelayCommand(ResetFilters, null);
            CloseTradeCommand = new RelayCommand(CloseTrade, null);
        }

        /// <summary>
        ///     Closes the trade.
        /// </summary>
        private void CloseTrade()
        {
           // var row = SelectedRow;
           //var wrapper = new IbClient();
           // wrapper.ClientSocket.eConnect("127.0.0.1", row.Port, 9999);
           // ReqGlobalCancel(wrapper);
           // Trade.PlaceMarketTrade(row.Instrument.Symbol, (double) row.Quantity, wrapper);
           // wrapper.ClientSocket.eDisconnect();
        }

        /// <summary>
        ///     Reqs the global cancel.
        /// </summary>
        /// <param name="wrapper">The wrapper.</param>
        //public static void ReqGlobalCancel(IbClient wrapper)
        //{
        //    wrapper.ClientSocket.reqGlobalCancel();
            
        //}

        /// <summary>
        ///     Filters by account.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FilterEventArgs" /> instance containing the event data.</param>
        private void FilterByAccount(object sender, FilterEventArgs e)
        {
            // see Notes on Filter Methods:
            if (e.Item is LiveTrade)
            {
                var src = (LiveTrade) e.Item;
                if (src == null)
                    e.Accepted = false;
                else if (string.Compare(SelectedAccount, src.Account.AccountNumber) != 0)
                    e.Accepted = false;
            }
        }

        /// <summary>
        ///     Adds the account filter.
        /// </summary>
        private void AddAccountFilter()
        {
            if (CanRemoveAccountFilter)
            {
                Cvs.Filter -= FilterByAccount;
                Cvs.Filter += FilterByAccount;
            }
            else
            {
                Cvs.Filter += FilterByAccount;

                CanRemoveAccountFilter = true;
            }
        }

        /// <summary>
        ///     Resets all filters.
        /// </summary>
        private void ResetFilters()
        {
            RemoveAccountFilter();
        }

        /// <summary>
        ///     Removes the account filter.
        /// </summary>
        public void RemoveAccountFilter()
        {
            if (CanRemoveAccountFilter)
            {
                Cvs.Filter -= FilterByAccount;

                SelectedAccount = null;
                CanRemoveAccountFilter = false;
            }
        }

        /// <summary>
        ///     Applies the filter.
        /// </summary>
        /// <param name="field">The field.</param>
        private void ApplyFilter(FilterField field)
        {
            switch (field)
            {
                case FilterField.Account:
                    AddAccountFilter();
                    break;

                default:
                    break;
            }
        }
    }
}