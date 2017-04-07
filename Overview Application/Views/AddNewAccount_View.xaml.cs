using Common;
using DataAccess;
using Microsoft.Practices.ServiceLocation;
using OverviewApp.ViewModels;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Timer = System.Threading.Timer;

namespace OverviewApp.Views
{
    /// <summary>
    /// Interaction logic for AddNewAccount_View.xaml
    /// </summary>
    public partial class AddNewAccountView : Window, IViewFor<AddNewAccountViewModel>
    {
        public AddNewAccountView(Account account)
        {
            ViewModel = new AddNewAccountViewModel(ServiceLocator.Current.GetInstance<MyDBContext>(), account);
            DataContext = ViewModel;
            InitializeComponent();
           // this.WhenAnyObservable(x => x.ViewModel.SaveCommand).Subscribe(x => WaitAndClose());
            
            this.WhenAnyObservable(x => x.ViewModel.CancelCommand).Subscribe(x => Close());
        }

        private async void WaitAndClose()
        {
            await Task.Delay(2000);
            Close();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AddNewAccountViewModel)value; }
        }

        public AddNewAccountViewModel ViewModel { get; set; }
    }
}