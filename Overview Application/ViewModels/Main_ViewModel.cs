using System.Windows.Input;
using DataAccess;
using DataStructures;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
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
        private readonly ILogger logger;
        private readonly IUnityContainer container;

        #region Fields

        private ICommand openDatabaseSettingsMenuCommand;

        private ICommand refreshPeopleMenuCommand;

        private string statusBarMessage;

        #endregion

        #region

        public MainViewModel(IDataService dataService, ILogger logger,IUnityContainer container) : base(dataService, logger)
        {
            this.logger = logger;
            this.container = container;
            // This will register our method with the Messenger class for incoming 
            // messages of type StatusMessage. So now we can send a StatusMessage from
            // any place in our application, it'l end up here, we'll update the string
            // we use to bind to our MainWindow status bar string, and wualla, magic
            // just happened.
            Messenger.Default.Register<StatusMessage>(this, msg => StatusBarMessage = msg.NewStatus);

            // This is how you can have some design time data
            if (IsInDesignMode)
            {
                StatusBarMessage = "Status in design";
            }
            else
            {
                StatusBarMessage = "Ready to rock and roll.";
            }
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

        public DatabaseConnectionViewModel DatabaseConnectionVm
            => ServiceLocator.Current.GetInstance<DatabaseConnectionViewModel>();

        /// <summary>
        ///     Used to bind any incoming status messages, to the MainWindow status bar.
        /// </summary>
        public string StatusBarMessage
        {
            get { return statusBarMessage; }
            set
            {
                if (value == statusBarMessage) return;
                statusBarMessage = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenDatabaseSettingsCommand => openDatabaseSettingsMenuCommand ??
                                                        (openDatabaseSettingsMenuCommand =
                                                            new RelayCommand<string>(
                                                                Execute_OpenDatabaseSettingsWindow,
                                                                CanExecute_OpenDatabaseSetttingsWindow));

        /// <summary>
        ///     This is in order to bind the command that we have in the MainWindow.
        ///     The Command is always enabled (so the can execute just returns true),
        ///     And it will send a message (that will be received by the Random View Model.
        /// </summary>
        public ICommand RefreshPeopleMenuCommand => refreshPeopleMenuCommand ??
                                                     (refreshPeopleMenuCommand =
                                                         new RelayCommand<string>(Execute_RefreshPeopleMenu,
                                                             CanExecute_RefreshPeopleMenu));

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