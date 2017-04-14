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
            //ContainerHelperDa.Configure(connectionString);
            //var container = new UnityContainer();
            container.RegisterType<MainViewModel>();
            container.RegisterType<SummaryViewModel>();
            container.RegisterType<AccountViewModel>();
            container.RegisterType<EquityViewModel>();
            // container.RegisterType<BarsViewModel>();}}
           
            container.RegisterType<StrategyViewModel>();
            container.RegisterType<CloseTradesViewModel>();
           
            container.RegisterType<IMyDbContext, MyDBContext>();
            container.RegisterType<IDataDBContext, DataDBContext>();

            //container.RegisterType<IMyDbContext, MyDBContext>(new InjectionConstructor(DBUtils.GetConnectionStringFromAppConfig(Settings.Default.allPurposeDatabaseName)));

            //container.RegisterType<IDataDBContext, DataDBContext>(new InjectionConstructor(DBUtils.GetConnectionStringFromAppConfig(Settings.Default.dataDatabaseName)));
        }
    }
}