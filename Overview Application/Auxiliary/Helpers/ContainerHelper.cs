﻿using DataAccess;
using DataAccess.Helpers;
using DataStructures;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.Unity;
using OverviewApp.Logger;
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
           container.RegisterType<IDataService, MySQLStorage>();
           container.RegisterType<SummaryViewModel>();
           container.RegisterType<AccountViewModel>();
           container.RegisterType<EquityViewModel>();
           container.RegisterType<BarsViewModel>();
           container.RegisterType<MatlabValueViewModel>();
           container.RegisterType<StrategyViewModel>();
           container.RegisterType<CloseTradesViewModel>();
           container.RegisterType<DatabaseConnectionViewModel>();
           container.RegisterType<ILogger, Logger.Logger>();
           container.RegisterType<IAttributesHelper, AttributesHelper>();
        }
    }
}
