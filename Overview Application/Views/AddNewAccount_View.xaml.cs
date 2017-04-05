using System;
using System.Windows;
using EntityData;
using Microsoft.Practices.ServiceLocation;
using OverviewApp.ViewModels;
using QDMS;
using ReactiveUI;


namespace OverviewApp.Views
{
    /// <summary>
    /// Interaction logic for AddNewAccount_View.xaml
    /// </summary>
    public partial class AddNewAccountView : Window, IViewFor<AddNewAccountViewModel>
    {
        public AddNewAccountView(Account account=null)
        {
            var myDbContext = ServiceLocator.Current.GetInstance<IMyDbContext>();
            ViewModel = new AddNewAccountViewModel(myDbContext, account);
            
            DataContext = ViewModel;
            InitializeComponent();

            this.WhenAnyObservable(x => x.ViewModel.CancelCommand).Subscribe(x => Hide());
            

        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AddNewAccountViewModel) value; }
        }

        public AddNewAccountViewModel ViewModel { get; set; }
    }
}
