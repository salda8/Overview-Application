﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Data;
using DataAccess;
using DataStructures;
using EntityData;
using QDMS;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Models;
using ReactiveUI;


namespace OverviewApp.ViewModels
{
    public class StrategyViewModel : MyBaseViewModel
    {
       #region Fields

        private string parameters;
        private Strategy selectedStrategy;
        private ObservableCollection<string> strategy;
        private ReactiveList<Strategy> strategyCollection;
        private DateTime lastUpdate = DateTime.Now;
        public List<string> StrategyList = new List<string>();
        private ReactiveCommand<Unit, Unit> launchStrategyCommand;

        #endregion

        #region

        public StrategyViewModel(IMyDbContext
            dataService, ILogger logger) : base(dataService, logger)
        {
            LoadData();
          
           
            Parameters = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dui." + Environment.NewLine +
                         "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dui." + Environment.NewLine +
                         "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dui." + Environment.NewLine;
            
            Messenger.Default.Register<ViewCollectionViewSourceMessageToken>(this,
                Handle_ViewCollectionViewSourceMessageToken);
        }

        #endregion

        #region Properties

        public ReactiveList<Strategy> StrategyCollection
        {
            get { return strategyCollection; }
            set { this.RaiseAndSetIfChanged(ref strategyCollection, value); }
        }

        public ObservableCollection<string> Strategy
        {
            get { return strategy; }
            set { this.RaiseAndSetIfChanged(ref strategy, value); }
        }

        public Strategy SelectedStrategy
        {
            get { return selectedStrategy; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedStrategy, value);
            }
        }

        public string Parameters
        {
            get { return parameters; }
            set
            {
               this.RaiseAndSetIfChanged(ref parameters,value);
            }
        }

        public ReactiveCommand<Unit, Unit> LaunchStrategyCommand
            => launchStrategyCommand ?? (launchStrategyCommand = ReactiveCommand.Create(StartProcess));
        
       

        #endregion

        private void Handle_ViewCollectionViewSourceMessageToken(ViewCollectionViewSourceMessageToken obj)
        {
        }

        private void LoadData()
        {
            StrategyCollection = new ReactiveList<Strategy>(Context.Strategies.ToList());
            Strategy = new ObservableCollection<string>();

            foreach (var strat in StrategyCollection)
            {
                Strategy.Add(strat.StrategyName + " | " + strat.CalmariRatio + " | " + strat.BacktestDrawDown + " | " +
                             strat.BacktestProfit + " | " + strat.BacktestPeriod + " | " + strat.Instrument.Symbol);
            }

            SelectedStrategy = strategyCollection.FirstOrDefault();

        }

        public void StartProcess()
        {
            var build = new Process
            {
                StartInfo =
                {
                    Arguments = parameters,
                    FileName = SelectedStrategy.Filepath,
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