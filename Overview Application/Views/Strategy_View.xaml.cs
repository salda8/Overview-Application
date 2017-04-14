using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;

namespace OverviewApp.Views
{
    /// <summary>
    ///     Interaction logic for Strategy_View.xaml
    /// </summary>
    public partial class StrategyView : UserControl
    {
        #region

        public StrategyView()
        {
            InitializeComponent();
            Messenger.Default.Send(new Helpers.ViewCollectionViewSourceMessageToken
            {
                StrategyCollectionViewSource = (CollectionViewSource) Resources["ST_CVS"]
            });
        }

        #endregion
    }
}