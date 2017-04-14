using Common;
using DataAccess;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using NLog;
using OverviewApp.Views;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using Common.EntityModels;
using StatusMessage = OverviewApp.Helpers.StatusMessage;

namespace OverviewApp.ViewModels
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ReactiveObject
    {
        #region Fields
        protected static NLog.Logger Logger = LogManager.GetCurrentClassLogger();
        private string statusBarMessage;
        private ReactiveCommand<object, Unit> addNewAccountCommand;
        private ReactiveCommand<object, Unit> addNewStrategyCommand;
        public ConcurrentNotifierBlockingList<LogEventInfo> LogMessages { get; set; }

        #endregion Fields

        #region

        public MainViewModel()
        {
            MappingConfiguration.Register();
            // This will register our method with the Messenger class for incoming
            // messages of type StatusMessage. So now we can send a StatusMessage from
            // any place in our application, it'l end up here, we'll update the string
            // we use to bind to our MainWindow status bar string, and wualla, magic
            // just happened.
            Messenger.Default.Register<Helpers.StatusMessage>(this, msg => StatusBarMessage = msg.NewStatus);
            LogMessages = new ConcurrentNotifierBlockingList<LogEventInfo>();

            StatusBarMessage = "Status in design";

            var myDbContext = ServiceLocator.Current.GetInstance<MyDBContext>();
            using (myDbContext)
            {
                var accounts = myDbContext.Account.ToList();
                if (accounts.Count > 0)
                {
                    AccountsList.AddRange(accounts);
                }
                var strategies = myDbContext.Strategy.ToList();
                if (strategies.Count > 0)
                {
                    StrategyList.AddRange(strategies);
                }
            }
        }

        public ReactiveList<Strategy> StrategyList { get; set; } = new ReactiveList<Strategy>();

        public ReactiveList<Account> AccountsList { get; set; } = new ReactiveList<Account>();

        #endregion

        #region Properties

        public SummaryViewModel SummaryVm => ServiceLocator.Current.GetInstance<SummaryViewModel>();

        public AccountViewModel AccountVm => ServiceLocator.Current.GetInstance<AccountViewModel>();

        public EquityViewModel EquityVm => ServiceLocator.Current.GetInstance<EquityViewModel>();

        //public BarsViewModel BarsVm => ServiceLocator.Current.GetInstance<BarsViewModel>();

        //public MatlabValueViewModel MatlabValueVm => ServiceLocator.Current.GetInstance<MatlabValueViewModel>();

        //public StrategyViewModel StrategyVm => ServiceLocator.Current.GetInstance<StrategyViewModel>();

        public AddEditStrategyViewModel AddEditAccountVm => ServiceLocator.Current.GetInstance<AddEditStrategyViewModel>();

        //public CloseTradesViewModel CloseTradesVm => ServiceLocator.Current.GetInstance<CloseTradesViewModel>();

        /// <summary>
        ///     Used to bind any incoming status messages, to the MainWindow status bar.
        /// </summary>
        public string StatusBarMessage
        {
            get { return statusBarMessage; }
            set { this.RaiseAndSetIfChanged(ref statusBarMessage, value); }
        }

        public ReactiveCommand<object, Unit> AddNewStrategyCommand
            =>
                addNewStrategyCommand ??
                (addNewStrategyCommand = ReactiveCommand.Create<object>(AddNewStrategy));

        private void AddNewStrategy(object strategy)
        {
            var window = new AddEditStrategy((Strategy)strategy);
            window.ShowDialog();
            RefreshStrategyCollection();
        }

        private void RefreshStrategyCollection()
        {
            var myDbContext = ServiceLocator.Current.GetInstance<MyDBContext>();
            using (myDbContext)
            {
                var strategies = myDbContext.Strategy.ToList();
                if (strategies.Count > 0)
                {
                    StrategyList.Clear();
                    foreach (Strategy strategy in strategies)
                    {
                        StrategyList.Add(strategy);
                    }
                    
                }
            }
        }

        private void RefreshAccountCollection()
        {
            var myDbContext = ServiceLocator.Current.GetInstance<MyDBContext>();
            using (myDbContext)
            {
                var accounts = myDbContext.Account.ToList();
                if (accounts.Count > 0)
                {
                    AccountsList.Clear();
                    foreach (Account account in accounts)
                    {
                        AccountsList.Add(account);
                    }
                   
                }
            }
        }

        public ReactiveCommand<object, Unit> AddNewAccountCommand
            =>
                addNewAccountCommand ??
                (addNewAccountCommand = ReactiveCommand.Create<object>(AddNewAccount));

        private void AddNewAccount(object param)
        {
            var window = new AddNewAccountView((Account)param);
            window.ShowDialog();
            RefreshAccountCollection();
        }

        #endregion
    }
}