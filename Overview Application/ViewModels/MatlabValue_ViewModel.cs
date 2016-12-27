using System.Collections.ObjectModel;
using System.Windows.Data;
using DataAccess;
using DataStructures;
using DataStructures.POCO;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
using ILogger = DataStructures.ILogger;

namespace OverviewApp.ViewModels
{
    public class MatlabValueViewModel : MyBaseViewModel
    {
        private readonly IDataService dataService;
        private readonly ILogger logger;

        #region Fields

        private ObservableCollection<Matlabvalue> matlabvaluesCollection;

        #endregion

        #region

        public MatlabValueViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            this.dataService = dataService;
            this.logger = logger;
           
            LoadData();
            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the IDownloadDataService member
        /// </summary>
        internal IDataService DataService { get; set; }

        private CollectionViewSource Mcvs { get; set; }

        public ObservableCollection<Matlabvalue> MatlabValueCollection
        {
            get { return matlabvaluesCollection; }
            set
            {
                if (Equals(value, matlabvaluesCollection)) return;
                matlabvaluesCollection = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private void LoadData()
        {
            var mlc = dataService.GetMatlabvalues();
            MatlabValueCollection = new ObservableCollection<Matlabvalue>(mlc);
        }

        /// <summary>
        ///     This method handles a message recieved from the View which enables a reference to the
        ///     instantiated CollectionViewSource to be used in the ViewModel.
        /// </summary>
        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken token)
        {
            if (token.LiveTradesCollectionViewSource != null)
            {
                Mcvs = token.MatlabValuesCollectionViewSource;
            }
        }
    }
}