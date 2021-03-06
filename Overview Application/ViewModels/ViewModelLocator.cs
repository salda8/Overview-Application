﻿/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:MVVM_Template_Project.ViewModels"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System.Diagnostics.CodeAnalysis;
using DataAccess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Design_Time;
using OverviewApp.Models;
using OverviewApp.Properties;

namespace OverviewApp.ViewModels
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class ViewModelLocator
    {
        #region

        static ViewModelLocator()
        {

            var container = new UnityContainer();
            UnityServiceLocator locator = new UnityServiceLocator(container);
            ContainerHelper.Configure(container, "");

            ServiceLocator.SetLocatorProvider(() => locator);
            //var ioc = new SimpleIoc();

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else { 
            //    SimpleIoc.Default.Register<IDataService, DataService>();
               
              
            //}

            

        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the Main property.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public SummaryViewModel SummaryVm => ServiceLocator.Current.GetInstance<SummaryViewModel>();

        public AccountViewModel AccountVm => ServiceLocator.Current.GetInstance<AccountViewModel>();

        public EquityViewModel EquityVm => ServiceLocator.Current.GetInstance<EquityViewModel>();

        public BarsViewModel BarsVm => ServiceLocator.Current.GetInstance<BarsViewModel>();

        public MatlabValueViewModel MatlabValueVm => ServiceLocator.Current.GetInstance<MatlabValueViewModel>();

        public StrategyViewModel StrategyVm => ServiceLocator.Current.GetInstance<StrategyViewModel>();

        public CloseTradesViewModel CloseTradesVm => ServiceLocator.Current.GetInstance<CloseTradesViewModel>();

        public DatabaseConnectionViewModel DatabaseConnectionVm
            => ServiceLocator.Current.GetInstance<DatabaseConnectionViewModel>();

        #endregion

        /// <summary>
        ///     Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}