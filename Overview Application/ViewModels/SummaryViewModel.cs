using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using QDMS;
using EntityData;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using ReactiveUI;
using ILogger = DataStructures.ILogger;

namespace OverviewApp.ViewModels
{
    public class SummaryViewModel : MyBaseViewModel
    {
        #region Fields

        

       

        private ICommand refreshSummaryCommand;

        private ReactiveList<PortfolioSummary> summaryCollection;

        #endregion

        #region

        /// <summary>
        ///     Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        public SummaryViewModel(IMyDbContext context, ILogger logger) : base(context, logger)
        {
          
            SummaryCollection = new ReactiveList<PortfolioSummary>(this.Context.PortfolioSummaries.ToList());

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
            SummaryCollection = new ReactiveList<PortfolioSummary>(Context.PortfolioSummaries.ToList());
            var msg = "refreshed.";
            OverviewApp.Auxiliary.StatusSetter.SetStatus(msg);
        }
    }
}