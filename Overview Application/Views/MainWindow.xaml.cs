using NLog;
using NLog.Targets;
using OverviewApp.Properties;
using OverviewApp.ViewModels;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using OverviewApp.Helpers;

namespace OverviewApp.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            DBUtils.CheckDBConnection();

            SetLogDirectory();

            DBUtils.SetConnectionString();

            InitializeComponent();
            //Log unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += AppDomain_CurrentDomain_UnhandledException;
            
            var target = LogManager.Configuration.AllTargets.Single(x => x.Name == "myTarget");
           

            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void AppDomain_CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Error((Exception)e.ExceptionObject, "Unhandled exception");
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        public MainViewModel ViewModel { get; set; }

        private void SetLogDirectory()
        {
            if (Directory.Exists(Settings.Default.logDirectory))
            {
                ((FileTarget)LogManager.Configuration.FindTargetByName("logfile")).FileName =
                    Settings.Default.logDirectory + "Log.log";
            }
            else
            {
                Directory.CreateDirectory(Properties.Settings.Default.logDirectory);
                ((FileTarget)LogManager.Configuration.FindTargetByName("logfile")).FileName =
                    Properties.Settings.Default.logDirectory + "Log.log";
            }
        }
    }
}