using DataAccess;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.TradingEntitiesPl;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Common;
using Common.Interfaces;

namespace OverviewApp.ViewModels
{
    public class MatlabValueViewModel : MyBaseViewModel
    {
        #region Fields

        private ObservableCollection<MatlabvaluePl> matlabvaluesCollection;

        #endregion Fields

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

        public ObservableCollection<MatlabvaluePl> MatlabValueCollection
        {
            get { return matlabvaluesCollection; }
            set { this.RaiseAndSetIfChanged(ref matlabvaluesCollection, value); }
        }

        #endregion

        private void LoadData()
        {
            var mlc = Context.MatlabValue;
            // MatlabValueCollection = mlc.MapMapTo<>new ObservableCollection<MatlabvaluePl>(ExpressMapper.Mapper.Map(mlc, typeof(List<MatlabvaluePl>)));
        }

        /// <summary>
        ///     This method handles a message received from the View which enables a reference to the
        ///     instantiated CollectionViewSource to be used in the ViewModel.
        /// </summary>
        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken token)
        {
            if (token.MatlabValuesCollectionViewSource != null)
            {
                Mcvs = token.MatlabValuesCollectionViewSource;
            }
        }
    }
}