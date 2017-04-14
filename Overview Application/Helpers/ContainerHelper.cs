using Common.Interfaces;
using DataAccess;
using Microsoft.Practices.Unity;
using OverviewApp.ViewModels;

namespace OverviewApp.Helpers
{
    public static class ContainerHelper
    {
        public static void Configure(UnityContainer container, string connectionString)
        {
            container.RegisterType<MainViewModel>();
            container.RegisterType<SummaryViewModel>();
            container.RegisterType<AccountViewModel>();
            container.RegisterType<EquityViewModel>();
            // container.RegisterType<BarsViewModel>();}}
            container.RegisterType<StrategyViewModel>();
            container.RegisterType<IMyDbContext, MyDBContext>();
            container.RegisterType<IDataDBContext, DataDBContext>();
        }
    }
}