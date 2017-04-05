using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EntityData;
using Microsoft.Practices.ServiceLocation;
using OverviewApp.ViewModels;
using QDMS;
using ReactiveUI;

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
            ViewModel = new AddEditStrategyViewModel(myDbContext,  strategy);
            DataContext = ViewModel;
            InitializeComponent();

            this.WhenAnyObservable(x => x.ViewModel.CancelCommand).Subscribe(x => Hide());
           

           

        }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AddEditStrategyViewModel) value; }
        }

        public AddEditStrategyViewModel ViewModel { get; set; }
    }
}
