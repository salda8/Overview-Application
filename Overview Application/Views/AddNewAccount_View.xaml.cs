using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OverviewApp.ViewModels;
using ReactiveUI;


namespace OverviewApp.Views
{
    /// <summary>
    /// Interaction logic for AddNewAccount_View.xaml
    /// </summary>
    public partial class AddNewAccount_View : Window, IViewFor<AddNewAccountViewModel>
    {
        public AddNewAccount_View()
        {
            ViewModel = new AddNewAccountViewModel();
            DataContext = ViewModel;
            InitializeComponent();

        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AddNewAccountViewModel) value; }
        }

        public AddNewAccountViewModel ViewModel { get; set; }
    }
}
