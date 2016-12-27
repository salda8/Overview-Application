using System.Runtime.CompilerServices;
using DataAccess;
using DataStructures;
using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using OverviewApp.Logger;
using OverviewApp.Models;
using OverviewApp.Properties;

namespace OverviewApp.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MyBaseViewModel : ViewModelBase
    {
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUnityContainer container;

        public MyBaseViewModel(IDataService dataService,ILogger logger)// IUnityContainer container)
        {
            this.dataService = dataService;
            this.logger = logger;
           // this.container = container;
        }

        /// <summary>
        ///     This gives us the ReSharper option to transform an autoproperty into a property with change notification
        ///     Also leverages .net 4.5 callermembername attribute
        /// </summary>
        /// <param name="property">name of the property</param>
        [NotifyPropertyChangedInvocator]
        protected override void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            base.RaisePropertyChanged(property);
        }

        /*

		/// <summary>
		/// This gives us the ReSharper option to transform an autoproperty into a property with change notification
		/// Also leverages .net 4.5 callermembername attribute
		/// </summary>
		/// <param name="property">name of the property</param>
		[NotifyPropertyChangedInvocator]
		protected override void RaisePropertyChanging([CallerMemberName]string property = "")
		{
			base.RaisePropertyChanging(property);
		}
        */
    }
}