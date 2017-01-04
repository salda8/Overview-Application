using System;
using System.Collections;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using DataStructures;
using EntityData;
using MvvmValidation;
using ReactiveUI;

namespace OverviewApp.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MyValidatableBaseViewModel : ReactiveObject, IValidatable, INotifyDataErrorInfo
    {
        private ReactiveCommand<Unit, Unit> cancelCommand;

        protected IMyDbContext Context { get; }

        protected ILogger Logger { get; }

        public ReactiveCommand<Unit,Unit> CancelCommand
            => cancelCommand ?? (cancelCommand = ReactiveCommand.Create((() => { })));

        protected ValidationHelper Validator { get; }

        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; }

        public MyValidatableBaseViewModel(IMyDbContext context,ILogger logger)// IUnityContainer container)
        {
            Validator = new ValidationHelper();
            this.Context = context;
            this.Logger = logger;
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

#endregion
    }
}