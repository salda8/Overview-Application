using System.Reactive;
using System.Windows.Input;
using EntityData;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Views;
using ReactiveUI;
using ILogger = DataStructures.ILogger;

namespace OverviewApp.ViewModels
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : MyBaseViewModel
    {
       

        #region Fields

        private ICommand openDatabaseSettingsMenuCommand;

        private ICommand refreshPeopleMenuCommand;

        private string statusBarMessage;
        private ReactiveCommand<Unit, Unit> addNewAccountCommand;
        private ReactiveCommand<Unit, Unit> addNewStrategyCommand;

        #endregion

        #region

        public MainViewModel(IMyDbContext context, ILogger logger,IUnityContainer container) : base(context, logger)
        {
          
            // This will register our method with the Messenger class for incoming 
            // messages of type StatusMessage. So now we can send a StatusMessage from
            // any place in our application, it'l end up here, we'll update the string
            // we use to bind to our MainWindow status bar string, and wualla, magic
            // just happened.
            Messenger.Default.Register<StatusMessage>(this, msg => StatusBarMessage = msg.NewStatus);

            // This is how you can have some design time data
            
                StatusBarMessage = "Status in design";
            
        }

        #endregion

        #region Properties

        public SummaryViewModel SummaryVm => ServiceLocator.Current.GetInstance<SummaryViewModel>();

        public AccountViewModel AccountVm => ServiceLocator.Current.GetInstance<AccountViewModel>();

        public EquityViewModel EquityVm => ServiceLocator.Current.GetInstance<EquityViewModel>();

        public BarsViewModel BarsVm => ServiceLocator.Current.GetInstance<BarsViewModel>();

        public MatlabValueViewModel MatlabValueVm => ServiceLocator.Current.GetInstance<MatlabValueViewModel>();

        public StrategyViewModel StrategyVm => ServiceLocator.Current.GetInstance<StrategyViewModel>();

        public CloseTradesViewModel CloseTradesVm => ServiceLocator.Current.GetInstance<CloseTradesViewModel>();
        
        /// <summary>
        ///     Used to bind any incoming status messages, to the MainWindow status bar.
        /// </summary>
        public string StatusBarMessage
        {
            get { return statusBarMessage; }
            set { this.RaiseAndSetIfChanged(ref statusBarMessage, value); }
        }

     

        public ICommand RefreshPeopleMenuCommand => refreshPeopleMenuCommand ??
                                                     (refreshPeopleMenuCommand =
                                                         new RelayCommand<string>(Execute_RefreshPeopleMenu,
                                                             CanExecute_RefreshPeopleMenu));

        public ReactiveCommand<Unit, Unit> AddNewStrategyCommand
            =>
                addNewStrategyCommand ??
                (addNewStrategyCommand = ReactiveCommand.Create(AddNewStrategy));

        private static void AddNewStrategy()
        {
            var window = new AddEditStrategy(null);
            window.ShowDialog();
        }

        public ReactiveCommand<Unit, Unit> AddNewAccountCommand
            =>
                addNewAccountCommand ??
                (addNewAccountCommand = ReactiveCommand.Create(AddNewAccount));

        private static void AddNewAccount()
        {
            var window = new AddNewAccountView(null);
            window.ShowDialog();
        }

        #endregion

        private bool CanExecute_OpenDatabaseSetttingsWindow(string arg)
        {
            return true;
        }

        private void Execute_OpenDatabaseSettingsWindow(string obj)
        {
            Messenger.Default.Send(new DatabaseSettings());
            //DatabaseConnection_ViewModel =
            
        }


        /// <summary>
        ///     Can always exectue this
        /// </summary>
        private bool CanExecute_RefreshPeopleMenu(string aNumberAsString)
        {
            return true;
        }

        /// <summary>
        ///     This will send the message when someone hits the command on the menu.
        /// </summary>
        /// <param name="aNumberAsString">
        ///     In our case, hard codedd in the MenuItem paramater.
        ///     You can easily bind it to anything you want.
        /// </param>
        private void Execute_RefreshPeopleMenu(string aNumberAsString)
        {
            var peopleToFetch = int.Parse(aNumberAsString);
            Messenger.Default.Send(new RefreshPeople(peopleToFetch));
        }
    }
}