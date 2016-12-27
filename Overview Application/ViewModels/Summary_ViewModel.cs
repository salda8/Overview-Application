using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DataAccess;
using DataStructures;
using DataStructures.POCO;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
using ILogger = DataStructures.ILogger;

namespace OverviewApp.ViewModels
{
    public class SummaryViewModel : MyBaseViewModel
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly ILogger logger;

        private IEnumerable<int> refreshNamesOptions;

        private ICommand refreshSummaryCommand;

        private ObservableCollection<PortfolioSummary> summaryCollection;

        #endregion

        #region

        /// <summary>
        ///     Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        public SummaryViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            _dataService = dataService;
            this.logger = logger;

            SummaryCollection = _dataService.GetPortfolioSummary();

            // This will register our method with the Messenger class for incoming
            // messages of type RefreshPeople.
            Messenger.Default.Register<RefreshSummary>(this, msg => Execute_RefreshSummary());
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Will hold a random collection of people.
        ///     The "Person" resides in the "Models" namespace/folder for
        ///     this demo.
        /// </summary>
        public ObservableCollection<PortfolioSummary> SummaryCollection

        {
            get { return summaryCollection; }
            set
            {
                if (Equals(value, summaryCollection)) return;
                summaryCollection = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     This collection is the source for the combo box: "Cmb_NamesAmount" on the XAML.
        ///     It also serves it's selected index to the refresh names command, which will
        ///     disable the button if the selected item is lower than 3.
        ///     When we set it (basically, change the selected item in the combo box), we'll
        ///     ask the command manager to reevaluate the conditions to see if the button needs
        ///     to be active or disactive.
        /// </summary>
        public IEnumerable<int> RefreshNamesOptions
        {
            get { return refreshNamesOptions; }
            set
            {
                if (Equals(value, refreshNamesOptions)) return;
                refreshNamesOptions = value;
                RaisePropertyChanged();
                // Without the following line, the button will not be disabled if you'll select
                // 1 from the combo box. It might still work (as in, you won't be able to click it
                // but the change won't happen immediately).
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /*
		 * First way of using the relay commands and ICommand. 
		 * The Command is created when we first try to get it, saving us the need to
		 * initialize it in the constructor.
		 */

        /// <summary>
        ///     You'll bind to this from a button on the GUI.
        ///     Note the Execute    -> this will be run when the button is clicked
        ///     and the CanExecute  -> This will enable / disable the button to begin with
        ///     Also, we're passing an int through the binding in the XAML:
        ///     ----> CommandParameter="{Binding ElementName=Cmb_NamesAmount, Path=SelectedItem}"
        ///     which is an item from the list of ints we defined. so the RelayCommand takes an "int",
        ///     hence the syntax with the type of paramater we'll accept
        ///     ----> "_refreshNames_command = new RelayCommand..."
        /// </summary>
        public ICommand RefreshSummaryCommand => refreshSummaryCommand ??
                                                  (refreshSummaryCommand =
                                                      new RelayCommand(Execute_RefreshSummary, CanExecute_RefreshSummary))
            ;

        #endregion

        /// <summary>
        ///     If you select 1 in the drop down menu, the button will become disable.
        ///     Rather simple, but it's a place holder for whatever logic you might want.
        /// </summary>
        /// <param name="arg">We're passing an int from the xaml.</param>
        /// <returns></returns>
        private bool CanExecute_RefreshSummary()
        {
            return true;
        }

        /// <summary>
        ///     This happens when you click the button.
        /// </summary>
        /// <param name="arg"></param>
        private void Execute_RefreshSummary()
        {
            SummaryCollection = new ObservableCollection<PortfolioSummary>(_dataService.GetPortfolioSummary());
            var msg = "refreshed.";
            OverviewApp.Auxiliary.StatusSetter.SetStatus(msg);
        }
    }
}