using System.Reactive;
using DataStructures;
using EntityData;
using log4net.Core;
using ReactiveUI;

namespace OverviewApp.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MyBaseViewModel : ReactiveObject
    {
        private ReactiveCommand<Unit, Unit> cancelCommand;

        protected IMyDbContext Context { get; }

        protected ILogger Logger { get; }

        public ReactiveCommand<Unit,Unit> CancelCommand
            => cancelCommand ?? (cancelCommand = ReactiveCommand.Create((() => { })));
        

        public MyBaseViewModel(IMyDbContext context,Splat.ILogger logger)// IUnityContainer container)
        {
            this.Context = context;
            this.Logger = logger;
          
        }

       
    }
}