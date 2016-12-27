using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;

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
            Messenger.Default.Send(new ViewCollectionViewSourceMessageToken
            {
                StrategyCollectionViewSource = (CollectionViewSource) Resources["ST_CVS"]
            });
        }

        #endregion
    }
}