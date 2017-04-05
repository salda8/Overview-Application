using EntityData;
using Microsoft.Practices.Unity;
using OverviewApp.Properties;
using OverviewApp.ViewModels;

namespace OverviewApp.Auxiliary.Helpers
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
            container.RegisterType<BarsViewModel>();
            container.RegisterType<MatlabValueViewModel>();
            container.RegisterType<StrategyViewModel>();
            container.RegisterType<CloseTradesViewModel>();
            container.RegisterType<IAttributesHelper, AttributesHelper>();
            container.RegisterType<IMyDbContext, MyDBContext>();
            container.RegisterType<IDataDBContext, DataDBContext>();
            
            //container.RegisterType<IMyDbContext, MyDBContext>(new InjectionConstructor(DBUtils.GetConnectionStringFromAppConfig(Settings.Default.allPurposeDatabaseName)));

            //container.RegisterType<IDataDBContext, DataDBContext>(new InjectionConstructor(DBUtils.GetConnectionStringFromAppConfig(Settings.Default.dataDatabaseName)));
        }
    }
}