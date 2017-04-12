
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using ReactiveUI;
using System.Linq;
using System.Windows.Input;
using Common;
using Common.EntityModels;
using Common.Interfaces;
using DataAccess;


namespace OverviewApp.ViewModels
{
    public class SummaryViewModel : MyBaseViewModel
    {
        #region Fields

        private ICommand refreshSummaryCommand;

        private ReactiveList<PortfolioSummary> summaryCollection;

        #endregion Fields

        #region

        /// <summary>
        ///     Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        public SummaryViewModel(IMyDbContext context) : base(context)
        {
            SummaryCollection = new ReactiveList<PortfolioSummary>(Context.PortfolioSummary.ToList());

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
        public ReactiveList<PortfolioSummary> SummaryCollection

        {
            get { return summaryCollection; }
            set { this.RaiseAndSetIfChanged(ref summaryCollection, value); }
        }

        public ICommand RefreshSummaryCommand => refreshSummaryCommand ??
                                                  (refreshSummaryCommand =
                                                      new RelayCommand(Execute_RefreshSummary, CanExecute_RefreshSummary))
            ;

        #endregion

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
            SummaryCollection = new ReactiveList<PortfolioSummary>(Context.PortfolioSummary.ToList());
            var msg = "refreshed.";
            Auxiliary.StatusSetter.SetStatus(msg);
        }
    }
}