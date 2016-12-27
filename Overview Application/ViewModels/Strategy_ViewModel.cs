using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using DataAccess;
using DataStructures;
using DataStructures.POCO;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
using ILogger = DataStructures.ILogger;

namespace OverviewApp.ViewModels
{
    public class StrategyViewModel : MyBaseViewModel
    {
        private readonly ILogger logger;

        #region Fields

        private string parameters;
        private int selectedStrategy;
        private ObservableCollection<string> strategy;
        private ObservableCollection<Strategy> strategyCollection;
        private DateTime lastUpdate = DateTime.Now;
        public List<string> StrategyList = new List<string>();

        #endregion

        #region

        public StrategyViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            this.logger = logger;
            SelectedStrategy = -1;
            DataService = dataService;
            InitializeCommands();
            Parameters = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dui." + Environment.NewLine +
                         "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dui." + Environment.NewLine +
                         "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dui." + Environment.NewLine;
            LoadData();
            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        public ObservableCollection<Strategy> StrategyCollection
        {
            get { return strategyCollection; }
            set
            {
                if (Equals(value, strategyCollection)) return;
                strategyCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> Strategy
        {
            get { return strategy; }
            set
            {
                if (strategy == value)
                    return;
                strategy = value;

                RaisePropertyChanged();
            }
        }

        public int SelectedStrategy
        {
            get { return selectedStrategy; }
            set
            {
                selectedStrategy = value;
                RaisePropertyChanged();
                // raise property change notification
            }
        }

        public string Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                RaisePropertyChanged();
            }
        }

        internal IDataService DataService { get; set; }
        private CollectionViewSource Stcvs { get; set; }
        public RelayCommand LaunchStrategyCommand { get; private set; }

        #endregion

        private void ApplyFilter(string selectedAccount)
        {
            throw new NotImplementedException();
        }

        private void InitializeCommands()
        {
            LaunchStrategyCommand = new RelayCommand(StartProcess, null);
        }

        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken obj)
        {
        }

        private void LoadData()
        {
            StrategyCollection = new ObservableCollection<Strategy>(DataService.GetStrategy());
            Strategy = new ObservableCollection<string>();

            foreach (var strat in StrategyCollection)
            {
                Strategy.Add(strat.StrategyName + " | " + strat.Calmari + " | " + strat.BacktestDrawDown + " | " +
                             strat.BacktestProfit + " | " + strat.BacktestPeriod + " | " + strat.Symbols);
            }
        }

        public void StartProcess()
        {
            var build = new Process
            {
                StartInfo =
                {
                    Arguments = parameters,
                    FileName = StrategyCollection[SelectedStrategy].Filepath,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false
                },
                EnableRaisingEvents = true
            };
            //build.StartInfo.WorkingDirectory = @"dir";
            //build.StartInfo.FileName = Directory.GetCurrentDirectory() + @"\IBSamples.exe";
            //build.StartInfo.RedirectStandardError = true;


            build.Start();
            //build.WaitForExit();
            //try
            //{
            //    using (var selProcess = new Process())
            //    {
            //        selProcess.StartInfo.FileName = Directory.GetCurrentDirectory() + @"\IBSamples.exe";
            //        selProcess.StartInfo.CreateNoWindow = true;
            //        selProcess.StartInfo.UseShellExecute = false;
            //        selProcess.StartInfo.RedirectStandardOutput = true;

            //        // Event Handler
            //        selProcess.OutputDataReceived += SortOutputHandler;

            //        // Start the process
            //        selProcess.Start();

            //        // Read output
            //        selProcess.BeginOutputReadLine();

            //    }
            //}
            //catch (Exception ex)
            //{
            //   ShowMessageOnPanel(Application.Current.MainWindow + ex.Message + "Error!" + MessageBoxButton.OK + MessageBoxImage.Error);
            //}
        }

        private void LaunchButtonClick()
        {
            //var dlg = new OpenFileDialog
            //{
            //    DefaultExt = ".exe",
            //    Filter = "Exe Files (*.exe)|*.exe"
            //};

            //Display OpenFileDialog by calling ShowDialog method
            //var result = dlg.ShowDialog();

            //Get the selected file name and display in a TextBox
            //if (result == true)
            //{
            //    Open document
            //    var filename = dlg.FileName;
            //    Filename.Text = filename;
            //    StartProcess(filename);

            //}

            var result = MessageBox.Show("Do you want to start trading on that account?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //var rowview = StrategyLaunchDataGrid.SelectedItem as DataRowView;
                //if (rowview != null)
                //{

                //    var filepath = rowview.Row[8].ToString();
                //    var account = rowview.Row[5].ToString();
                //    var port = rowview.Row[6].ToString();
                //    StartProcess(filepath, port, account);
                //}
                //var senderGrid = (DataGrid)sender;
                //if (senderGrid.Columns?.Count > 0)
                //{
                //    var filepath = senderGrid.Columns[0].ToString();
                //    var account=  senderGrid.Columns[5].ToString();
                //    var port = senderGrid.Columns[6].ToString();
                //    StartProcess(filepath, port, account);
                //}
            }
        }
    }
}