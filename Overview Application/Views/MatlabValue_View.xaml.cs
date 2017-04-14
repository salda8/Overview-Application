using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;

namespace OverviewApp.Views
{
    /// <summary>
    ///     Interaction logic for Matlabvalue_View.xaml
    /// </summary>
    public partial class MatlabvalueView : UserControl
    {
        #region

        public MatlabvalueView()
        {
            InitializeComponent();
            Messenger.Default.Send(new Helpers.ViewCollectionViewSourceMessageToken
            {
                MatlabValuesCollectionViewSource = (CollectionViewSource) Resources["M_CVS"]
            });
        }

        #endregion
    }
}