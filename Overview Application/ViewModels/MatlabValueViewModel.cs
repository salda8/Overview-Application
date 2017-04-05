using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using EntityData;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using QDMS;
using ReactiveUI;


namespace OverviewApp.ViewModels
{
    public class MatlabValueViewModel : MyBaseViewModel
    {
        #region Fields

        private ObservableCollection<Matlabvalue> matlabvaluesCollection;

        #endregion

        #region

        public MatlabValueViewModel(IMyDbContext context) : base(context)
        {
           
            LoadData();
            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

       

        private CollectionViewSource Mcvs { get; set; }

        public ObservableCollection<Matlabvalue> MatlabValueCollection
        {
            get { return matlabvaluesCollection; }
            set { this.RaiseAndSetIfChanged(ref matlabvaluesCollection, value); }
        }

        #endregion

        private void LoadData()
        {
            var mlc = Context.MatlabValue.ToList();
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