using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using DataStructures;
using EntityData;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmValidation;
using QDMS;
using ReactiveUI;
using ReactiveUI.Legacy;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace OverviewApp.ViewModels
{
    public class AddEditStrategyViewModel : MyValidatableBaseViewModel
    {
        
       
        private string windowTitle;
        
        private string addNewEditText;
       
        private readonly bool addingNew;
        private readonly Strategy originalStrategy;
        private Strategy strategy;
        private string strategyName;
        private decimal backTestDrawDown;
        private decimal backTestPeriod;
        private decimal backTestProfit;
        private decimal calmariRatio;
        private decimal dailyProfit;
        private decimal daysRunning;
        private string filePath;
        private Instrument selectedInstrument;
        private List<Instrument> instruments;
        private bool? isValid;
        private string validationErrorsString;
        private ReactiveCommand<Unit, Unit> saveCommand;
        private ReactiveCommand<Unit, Unit> openFileDialogCommand;

        public AddEditStrategyViewModel(IMyDbContext context, ILogger logger,Strategy strategy) : base(context, logger)
        {

            originalStrategy = strategy;
            
            

            if (strategy != null)
            {
                Strategy = strategy;
                WindowTitle = "Edit Strategy";
                AddNewEditText = "Save Changes";
                BacktestDrawDown = originalStrategy.BacktestDrawDown;
                StrategyName = originalStrategy.StrategyName;
                FilePath = originalStrategy.Filepath;
                BacktestDrawDown = originalStrategy.BacktestDrawDown;
                BacktestPeriod = originalStrategy.BacktestPeriod;
                BacktestProfit = originalStrategy.BacktestProfit;
                CalmariRatio = originalStrategy.CalmariRatio;
                DailyProfit = originalStrategy.DailyProfit;
                DaysRunning = originalStrategy.DaysRunning;
                SelectedInstrument = originalStrategy.Instrument;


            }
            else
            {
                WindowTitle = "Add new Strategy";
                AddNewEditText = "Add New";
                addingNew = true;

            }

           
        
            ConfigureValidationRules();
            Validator.ResultChanged += OnValidationResultChanged;


        }

        public List<Instrument> Instruments
            => instruments?? (instruments = new List<Instrument>(Context.Instruments.ToList()));

        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand=> openFileDialogCommand?? (openFileDialogCommand=ReactiveCommand.Create(OpenNewFileDialog));


        private void OpenNewFileDialog()
        {
            var dialog = new OpenFileDialog();
           
            var result = dialog.ShowDialog();
            if (result== DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
        }

        public ReactiveCommand<Unit, Unit> SaveCommand => saveCommand??(saveCommand= ReactiveCommand.Create(() =>
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
            {BacktestDrawDown = this.BacktestDrawDown,
            BacktestPeriod = this.BacktestPeriod,
            BacktestProfit = this.BacktestProfit,
            CalmariRatio = this.CalmariRatio,
            DailyProfit = this.DailyProfit,
            DaysRunning = this.DaysRunning,
            Filepath = this.FilePath,
            InstrumentID = SelectedInstrument.ID.Value,
            StrategyName = this.StrategyName
                

            };

            if (addingNew)
            {
               
                Context.Strategies.Add(strat);
                
            }
            else
            {
                Context.Strategies.Attach(Strategy);
               
                Context.Entry(originalStrategy).CurrentValues.SetValues(strat);
                
            }
            Context.SaveChanges();
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

        public decimal DaysRunning
        {
            get { return daysRunning; }
            set {this.RaiseAndSetIfChanged(ref  daysRunning, value); }
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
            Validator.AddRequiredRule(() => StrategyName, "Account Name is required");

            Validator.AddRule((string)(nameof(StrategyName)),
                 () =>
                 {
                     bool isAvailable =
                          Context.Strategies.Any(x => x.StrategyName == this.StrategyName);

                     return RuleResult.Assert(isAvailable,
                                              $"This strategy name {StrategyName} is present. Please choose a different one or edit existing one");
                 });

            
            Validator.AddRequiredRule(() => FilePath, "FilePath is required");
            Validator.AddRule((string)(nameof(FilePath)),
                () =>
                {
                    bool isAvailable =
                         Context.Strategies.Any(x => x.Filepath == this.FilePath);

                    return RuleResult.Assert(isAvailable,
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

        private async void Validate()
        {
            await ValidateAsync();
        }

        private async Task ValidateAsync()
        {
            var result = await Validator.ValidateAllAsync();

            UpdateValidationSummary(result);
        }
        #endregion

    }
}
