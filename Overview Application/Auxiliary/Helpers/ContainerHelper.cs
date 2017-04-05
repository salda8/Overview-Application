
using EntityData;
using Microsoft.Practices.Unity;
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
           container.RegisterType<IMyDbContext, MyDBContext>(
               new InjectionConstructor("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=qdms;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            container.RegisterType<IMyDbContext, MyDBContext>(
               new InjectionConstructor("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=qdmsData;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

        }
    }
}
