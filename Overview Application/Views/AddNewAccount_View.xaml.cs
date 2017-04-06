using EntityData;
using Microsoft.Practices.ServiceLocation;
using OverviewApp.ViewModels;
using QDMS;
using ReactiveUI;
using System;
using System.Windows;

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

            this.WhenAnyObservable(x => x.ViewModel.CancelCommand).Subscribe(x => Close());
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AddNewAccountViewModel)value; }
        }

        public AddNewAccountViewModel ViewModel { get; set; }
    }
}