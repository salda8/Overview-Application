using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;

namespace OverviewApp.Views
{
    /// <summary>
    ///     Interaction logic for Bars_View.xaml
    /// </summary>
    public partial class BarsView : UserControl
    {
        #region

        public BarsView()
        {
            InitializeComponent();
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                BarsCollectionViewSource = (CollectionViewSource) Resources["B_CVS"]
            });
        }

        #endregion
    }
}