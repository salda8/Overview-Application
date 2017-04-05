using System.Reactive;
using EntityData;
using NLog;
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

        protected static NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public ReactiveCommand<Unit,Unit> CancelCommand
            => cancelCommand ?? (cancelCommand = ReactiveCommand.Create((() => { })));
        

        public MyBaseViewModel(IMyDbContext context)
        {
            this.Context = context;
           
          
        }

       
    }
}