using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Forms;
using System.Windows.Input;
using DataStructures;
using EntityData;
using GalaSoft.MvvmLight.CommandWpf;
using QDMS;
using ReactiveUI;
using ReactiveUI.Legacy;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace OverviewApp.ViewModels
{
    public class AddEditStrategyViewModel : MyBaseViewModel
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

            SaveCommand = ReactiveCommand.Create(AddNewStrategy);
            OpenFileDialogCommand = ReactiveCommand.Create(OpenNewFileDialog);


        }

        public List<Instrument> Instruments
            => instruments?? (instruments = new List<Instrument>(Context.Instruments.ToList()));
        
        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; set; }

        private void OpenNewFileDialog()
        {
            var dialog = new OpenFileDialog();
           
            var result = dialog.ShowDialog();
            if (result== DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
        }

        public ReactiveCommand<Unit,Unit> SaveCommand { get; }


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
            set { this.RaiseAndSetIfChanged(ref strategyName, value); }
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
            set { this.RaiseAndSetIfChanged(ref filePath, value); }
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


    }
}
