using DataAccess;
using NLog;
using ReactiveUI;
using System.Reactive;

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

        public ReactiveCommand<Unit, Unit> CancelCommand
            => cancelCommand ?? (cancelCommand = ReactiveCommand.Create((() => { })));

        public MyBaseViewModel(IMyDbContext context)
        {
            Context = context;
        }
    }
}