using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Controls;
using System.Windows.Input;
using EntityData;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using NLog;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Views;
using QDMS;
using ReactiveUI;


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

        #endregion

        #region

        public MainViewModel()
        {
          
            // This will register our method with the Messenger class for incoming 
            // messages of type StatusMessage. So now we can send a StatusMessage from
            // any place in our application, it'l end up here, we'll update the string
            // we use to bind to our MainWindow status bar string, and wualla, magic
            // just happened.
            Messenger.Default.Register<StatusMessage>(this, msg => StatusBarMessage = msg.NewStatus);
            LogMessages = new ConcurrentNotifierBlockingList<LogEventInfo>();
            // This is how you can have some design time data

            StatusBarMessage = "Status in design";
            
            var myDbContext = ServiceLocator.Current.GetInstance<MyDBContext>();
            using (myDbContext)
            {
                AccountsList = myDbContext.Account.ToList();
                StrategyList = myDbContext.Strategy.ToList();


            }

        }

        public List<Strategy> StrategyList { get; set; }

        public List<Account> AccountsList { get; set; }

   

        #endregion

        #region Properties

        public SummaryViewModel SummaryVm => ServiceLocator.Current.GetInstance<SummaryViewModel>();

        public AccountViewModel AccountVm => ServiceLocator.Current.GetInstance<AccountViewModel>();

        public EquityViewModel EquityVm => ServiceLocator.Current.GetInstance<EquityViewModel>();

        //public BarsViewModel BarsVm => ServiceLocator.Current.GetInstance<BarsViewModel>();

        public MatlabValueViewModel MatlabValueVm => ServiceLocator.Current.GetInstance<MatlabValueViewModel>();

        public StrategyViewModel StrategyVm => ServiceLocator.Current.GetInstance<StrategyViewModel>();

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

        private static void AddNewStrategy(object strategy)
        {
            var window = new AddEditStrategy((Strategy)strategy);
            window.ShowDialog();
        }

        public ReactiveCommand<object, Unit> AddNewAccountCommand
            =>
                addNewAccountCommand ??
                (addNewAccountCommand = ReactiveCommand.Create<object>(AddNewAccount));

        private static void AddNewAccount(object param)
        {
            var window = new AddNewAccountView((Account)param);
            window.ShowDialog();
        }

        #endregion
    }
}