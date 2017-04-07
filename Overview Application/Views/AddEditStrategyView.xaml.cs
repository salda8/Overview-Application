using Common;
using DataAccess;
using Microsoft.Practices.ServiceLocation;
using OverviewApp.ViewModels;
using ReactiveUI;
using System;
using System.Windows;
namespace OverviewApp.Views
{
    /// <summary>
    /// Interaction logic for AddEditStrategy.xaml
    /// </summary>
    public partial class AddEditStrategy : Window, IViewFor<AddEditStrategyViewModel>
    {
        public AddEditStrategy(Strategy strategy)
        {
            var myDbContext = ServiceLocator.Current.GetInstance<IMyDbContext>();
            ViewModel = new AddEditStrategyViewModel(myDbContext, strategy);
            DataContext = ViewModel;
            InitializeComponent();

            this.WhenAnyObservable(x => x.ViewModel.CancelCommand).Subscribe(x => Close());
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AddEditStrategyViewModel)value; }
        }

        public AddEditStrategyViewModel ViewModel { get; set; }
    }
}