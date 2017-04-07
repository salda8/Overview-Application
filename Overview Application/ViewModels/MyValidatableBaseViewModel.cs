using System;
using System.Collections;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using DataAccess;
using MvvmValidation;
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
    public class MyValidatableBaseViewModel : ReactiveObject, IValidatable, INotifyDataErrorInfo, IDisposable
    {
        private ReactiveCommand<Unit, Unit> cancelCommand;

        protected IMyDbContext Context { get; }

        protected static NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public ReactiveCommand<Unit,Unit> CancelCommand
            => cancelCommand ?? (cancelCommand = ReactiveCommand.Create(Dispose));

        protected ValidationHelper Validator { get; }

        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; }

        public MyValidatableBaseViewModel(IMyDbContext context)// IUnityContainer container)
        {
            Validator = new ValidationHelper();
            Context = context;
           
            NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
            NotifyDataErrorInfoAdapter.ErrorsChanged += OnErrorsChanged;
        }

        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            // Notify the UI that the property has changed so that the validation error gets displayed (or removed).
            this.RaisePropertyChanged(e.PropertyName);
        }

        Task<ValidationResult> IValidatable.Validate()
        {
            return Validator.ValidateAllAsync();
        }

        #region Implementation of INotifyDataErrorInfo

        public IEnumerable GetErrors(string propertyName)
        {
            return NotifyDataErrorInfoAdapter.GetErrors(propertyName);
        }

        public bool HasErrors => NotifyDataErrorInfoAdapter.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { NotifyDataErrorInfoAdapter.ErrorsChanged += value; }
            remove { NotifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        }

        public void Dispose()
        {
            cancelCommand?.Dispose();
            Context.Dispose();
        }

        #endregion
    }
}