using Common;
using DataAccess;
using MvvmValidation;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Forms;
using Common.EntityModels;
using Common.Interfaces;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace OverviewApp.ViewModels
{
    public class AddEditStrategyViewModel : ValidateableBaseViewModel
    {
        private string windowTitle;

        private string addNewEditText;

        private readonly bool addingNew;
        private readonly Strategy originalStrategy;
        private Strategy strategy;
        private string strategyName;
        private decimal backTestDrawDown = 5000;
        private decimal backTestPeriod = 30;
        private decimal backTestProfit = 10000;
        private decimal calmariRatio = 2;
        private decimal dailyProfit;

        private string filePath;
        private Instrument selectedInstrument;
        private List<Instrument> instruments;
        private bool? isValid;
        private string validationErrorsString;
        private ReactiveCommand<Unit, Unit> saveCommand;
        private ReactiveCommand<Unit, Unit> openFileDialogCommand;

        public AddEditStrategyViewModel(IMyDbContext context, Strategy strategy) : base(context)
        {
            originalStrategy = strategy;

            if (strategy != null)
            {
                Strategy = strategy;
                WindowTitle = "Edit Strategy";
                AddNewEditText = "Edit";
                BacktestDrawDown = originalStrategy.BacktestDrawDown;
                StrategyName = originalStrategy.StrategyName;
                FilePath = originalStrategy.Filepath;
                BacktestDrawDown = originalStrategy.BacktestDrawDown;
                BacktestPeriod = originalStrategy.BacktestPeriod;
                BacktestProfit = originalStrategy.BacktestProfit;
                CalmariRatio = originalStrategy.CalmariRatio;

                SelectedInstrument = originalStrategy.Instrument;
                IsStrategyNameEditable = false;
            }
            else
            {
                WindowTitle = "Add new Strategy";
                AddNewEditText = "Add New";
                addingNew = true;
                IsStrategyNameEditable = true;
            }

            ConfigureValidationRules();
            Validator.ResultChanged += OnValidationResultChanged;
        }

        public bool IsStrategyNameEditable { get; set; }

        public List<Instrument> Instruments
            => instruments ?? (instruments = new List<Instrument>(Context.Instruments.ToList()));

        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand => openFileDialogCommand ?? (openFileDialogCommand = ReactiveCommand.Create(OpenNewFileDialog));

        private void OpenNewFileDialog()
        {
            var dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
        }

        public ReactiveCommand<Unit, Unit> SaveCommand => saveCommand ?? (saveCommand = ReactiveCommand.Create(() =>
           {
               {
                   Validate();

                   if (IsValid.GetValueOrDefault(false))
                   {
                       AddNewStrategy();
                   }
               }
           }));

        private void AddNewStrategy()
        {
            var strat = new Strategy()
            {
                BacktestDrawDown = BacktestDrawDown,
                BacktestPeriod = BacktestPeriod,
                BacktestProfit = BacktestProfit,
                CalmariRatio = CalmariRatio,
                Filepath = FilePath,
                InstrumentID = SelectedInstrument.ID.Value,
                StrategyName = StrategyName,
            };

            if (addingNew)
            {
                Context.Strategy.Add(strat);
                Context.SaveChanges();
            }
            else
            {
                var foundStrategy = Context.Strategy.Find(originalStrategy.ID);
                if (foundStrategy != null)
                {
                    Context.UpdateEntryValues(foundStrategy, strat);
                    Context.SaveChanges();
                }
            }
        }

        public string StrategyName
        {
            get { return strategyName; }
            set
            {
                this.RaiseAndSetIfChanged(ref strategyName, value);
                Validator.Validate((nameof(StrategyName)));
            }
        }

        public decimal BacktestDrawDown
        {
            get { return backTestDrawDown; }
            set { this.RaiseAndSetIfChanged(ref backTestDrawDown, value); }
        }

        public decimal BacktestPeriod
        {
            get { return backTestPeriod; }
            set { this.RaiseAndSetIfChanged(ref backTestPeriod, value); }
        }

        public decimal BacktestProfit
        {
            get { return backTestProfit; }
            set { this.RaiseAndSetIfChanged(ref backTestProfit, value); }
        }

        public decimal CalmariRatio
        {
            get { return calmariRatio; }
            set { this.RaiseAndSetIfChanged(ref calmariRatio, value); }
        }

        public decimal DailyProfit
        {
            get { return dailyProfit; }
            set { this.RaiseAndSetIfChanged(ref dailyProfit, value); }
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                this.RaiseAndSetIfChanged(ref filePath, value);
                Validator.Validate((nameof(FilePath)));
            }
        }

        public Instrument SelectedInstrument
        {
            get { return selectedInstrument; }
            set { this.RaiseAndSetIfChanged(ref selectedInstrument, value); }
        }

        public Strategy Strategy
        {
            get { return strategy; }
            set { this.RaiseAndSetIfChanged(ref strategy, value); }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set { this.RaiseAndSetIfChanged(ref windowTitle, value); }
        }

        public string AddNewEditText
        {
            get { return addNewEditText; }
            set { this.RaiseAndSetIfChanged(ref addNewEditText, value); }
        }

        #region Validation

        public string ValidationErrorsString
        {
            get { return validationErrorsString; }
            private set
            {
                this.RaiseAndSetIfChanged(ref validationErrorsString, value);
            }
        }

        public bool? IsValid
        {
            get { return isValid; }
            private set { this.RaiseAndSetIfChanged(ref isValid, value); }
        }

        private void ConfigureValidationRules()
        {
            Validator.AddRequiredRule(() => StrategyName, "Strategy Name is required");

            Validator.AddRule((string)(nameof(StrategyName)),
                 () =>
                 {
                     if (!IsStrategyNameEditable) return RuleResult.Valid();
                     bool isNotAvailable =
                          Context.Strategy.Any(x => x.StrategyName == StrategyName);

                     return RuleResult.Assert(!isNotAvailable,
                                              $"This strategy name {StrategyName} is present. Please choose a different one or edit existing one");
                 });

            //Validator.AddRequiredRule(() => FilePath, "FilePath is required");
            Validator.AddRule((string)(nameof(FilePath)),
                () =>
                {
                    if (string.IsNullOrEmpty(FilePath))
                    {
                        return RuleResult.Valid();
                    }
                    bool isAvailable =
                         Context.Strategy.Any(x => x.Filepath == FilePath);

                    return RuleResult.Assert(!isAvailable,
                                             $"This file is already used. Please choose a different strategy or edit existing one.");
                });
        }

        private void OnValidationResultChanged(object sender, ValidationResultChangedEventArgs e)
        {
            if (!IsValid.GetValueOrDefault(true))
            {
                ValidationResult validationResult = Validator.GetResult();

                UpdateValidationSummary(validationResult);
            }
        }

        private void UpdateValidationSummary(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ValidationErrorsString = validationResult.ToString();
        }

        private void Validate() => UpdateValidationSummary(Validator.ValidateAllAsync().Result);

        #endregion Validation
    }
}