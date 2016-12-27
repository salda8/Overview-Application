using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using OverviewApp.Auxiliary.Helpers;
using OverviewApp.Properties;
using System;
using System.Windows;
using DataAccess;
using DataStructures;
using OverviewApp.Logger;


namespace OverviewApp.ViewModels
{
    public class DatabaseConnectionViewModel : MyBaseViewModel
    {
        private readonly ILogger logger;

        #region Fields

        private bool dbSelectionTab = true;
        private bool mySqlTab;
        private bool sqlTab;
        private bool useMySql;

        private string mySqlUsername;
        private string mySqlPassword;
        private bool windowsAuthentication;
        private bool sqlServerAuthentication;
        private string sqlServerHost;
        private string sqlServerUsername;
        private string sqlServerPassword;
        private string mySqlHost;
        private Visibility mySqlTabVisibility = Visibility.Hidden;
        private Visibility sqlTabVisibility = Visibility.Hidden;

        #endregion Fields

        #region

        public DatabaseConnectionViewModel(IDataService dataService, ILogger logger) : base(dataService, logger)
        {
            this.logger = logger;
            //Hide the tab header
            //foreach (TabItem t in Tabs.Items)
            //{
            //    t.Visibility = Visibility.Collapsed;
            //}
            InitializeCommands();

            MySqlHost = Settings.Default.MySqlServerHost;
            MySqlUsername = Settings.Default.MySqlUsername;
            MySqlPassword = Settings.Default.MySqlServerPassword;

            WindowsAuthentication = Settings.Default.SqlServerUseWindowsAuthentication;
            SqlServerAuthentication = !Settings.Default.SqlServerUseWindowsAuthentication;

            SqlServerHost = Settings.Default.SqlServerHost;
            SqlServerUsername =
                string.IsNullOrEmpty(Settings.Default.SqlServerUsername)
                    ? Settings.Default.SqlServerUsername
                    : "(localdb)\\MSSQLLocalDB";
            SqlServerPassword = "asdf";
        }

        #endregion

        #region Properties

        public string SqlServerPassword
        {
            get { return sqlServerPassword; }
            set
            {
                sqlServerPassword = value;
                RaisePropertyChanged();
            }
        }

        public string SqlServerUsername
        {
            get { return sqlServerUsername; }
            set
            {
                sqlServerUsername = value;
                RaisePropertyChanged();
            }
        }

        public string SqlServerHost
        {
            get { return sqlServerHost; }
            set
            {
                sqlServerHost = value;
                RaisePropertyChanged();
            }
        }

        public bool SqlServerAuthentication
        {
            get { return sqlServerAuthentication; }
            set
            {
                sqlServerAuthentication = value;
                RaisePropertyChanged();
            }
        }

        public bool WindowsAuthentication
        {
            get { return windowsAuthentication; }
            set
            {
                windowsAuthentication = value;
                RaisePropertyChanged();
            }
        }

        public string MySqlPassword
        {
            get { return mySqlPassword; }
            set
            {
                mySqlPassword = value;
                RaisePropertyChanged();
            }
        }

        public string MySqlUsername
        {
            get { return mySqlUsername; }
            set
            {
                mySqlUsername = value;
                RaisePropertyChanged();
            }
        }

        public string MySqlHost
        {
            get { return mySqlHost; }
            set
            {
                mySqlHost = value;
                RaisePropertyChanged();
            }
        }

        public bool UseMySql { get; set; }

        public bool UseSql { get; set; }

        public RelayCommand NextTabItemCommand { get; set; }

        public bool SqlTab
        {
            get { return sqlTab; }
            set
            {
                sqlTab = value;
                RaisePropertyChanged();
            }
        }

        public bool MySqlTab
        {
            get { return mySqlTab; }
            set
            {
                mySqlTab = value;
                RaisePropertyChanged();
            }
        }

        public bool DbSelectionTab
        {
            get { return dbSelectionTab; }
            set
            {
                dbSelectionTab = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand MySqlOkCommand { get; set; }

        public RelayCommand MySqlTestCommand { get; set; }
        public RelayCommand SqlOkCommand { get; set; }

        public RelayCommand SqlTestCommand { get; set; }

        public Visibility MySqlTabVisibility
        {
            get { return mySqlTabVisibility; }
            set { mySqlTabVisibility = value; }
        }

        public Visibility SqlTabVisibility
        {
            get { return sqlTabVisibility; }
            set { sqlTabVisibility = value; }
        }

        public Visibility DbSelectionTabVisibility { get; set; } = Visibility.Visible;

        #endregion

        private void InitializeCommands()
        {
            NextTabItemCommand = new RelayCommand(NextTab, null);
            MySqlOkCommand = new RelayCommand(MySqlOk, null);
            MySqlTestCommand = new RelayCommand(MySqlTestConnection, null);
            SqlOkCommand = new RelayCommand(SqlOk, null);
            SqlTestCommand = new RelayCommand(SqlTestConnection, null);
        }

        private void MySqlTestConnection()
        {
            //var connection = DbUtils.CreateMySqlConnection(
            //    server: MySqlHost,
            //    username: MySqlUsername,
            //    password: MySqlPassword,
            //    noDB: true);
            //try
            //{
            //    connection.Open();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Connection failed with error: " + ex.Message);
            //    return;
            //}

            //MessageBox.Show("Connection succeeded.");
            //connection.Close();
        }

        private void MySqlOk()
        {
            //var connection = DbUtils.CreateMySqlConnection(
            //    server: MySqlHost,
            //    username: MySqlUsername,
            //    password: MySqlPassword,
            //    noDB: true);
            //try
            //{
            //    connection.Open();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Connection failed with error: " + ex.Message);
            //    return;
            //}
            //connection.Close();

            Settings.Default.MySqlServerHost = MySqlHost;
            Settings.Default.MySqlUsername = MySqlUsername;
            Settings.Default.MySqlServerPassword = EncryptionUtils.Protect(MySqlPassword);
            Settings.Default.DatabaseType = "MySql";

            Settings.Default.Save();

            //if (SimpleIoc.Default.IsRegistered<IDataService>()) SimpleIoc.Default.Unregister<SqlServerStorage>();
            if (SimpleIoc.Default.IsRegistered<IDataService>()) SimpleIoc.Default.Unregister<IDataService>();

            SimpleIoc.Default.Register<IDataService, MySQLStorage>();
        }

        private void NextTab()
        {
            if (UseMySql)
            {
                MySqlTabVisibility = Visibility.Visible;
                DbSelectionTabVisibility = Visibility.Collapsed;

                MySqlTab = true;
            }
            else if (UseSql)
            {
                SqlTabVisibility = Visibility.Visible;
                DbSelectionTabVisibility = Visibility.Collapsed;
                SqlTab = true;
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            //depending on the choice, go to the appropriate database selection screen
            //if (MySqlRadiobtn.IsChecked.HasValue && MySqlRadiobtn.IsChecked.Value)
            //{
            //    Tabs.SelectedIndex = 1;
            //}
            //else
            //{
            //    Tabs.SelectedIndex = 2;
            //}
        }

        private void SqlOk()
        {
            var windowsAuth = WindowsAuthentication;
            var connection = DbUtils.CreateSqlServerConnection(
                server: SqlServerHost,
                username: SqlServerUsername,
                password: SqlServerPassword,
                useWindowsAuthentication: windowsAuth,
                noDB: true);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed with error: " + ex.Message);
                return;
            }

            if (WindowsAuthentication)
            {
                Settings.Default.SqlServerUseWindowsAuthentication = WindowsAuthentication;
            }
            Settings.Default.SqlServerHost = SqlServerHost;
            Settings.Default.SqlServerUsername = SqlServerUsername;
            Settings.Default.SqlServerPassword = EncryptionUtils.Protect(SqlServerPassword);
            Settings.Default.DatabaseType = "SqlServer";

            Settings.Default.Save();

            //VisualStyleElement.ToolTip.Close();
        }

        private void SqlTestConnection()
        {
            var windowsAuth = WindowsAuthentication;

            var connection = DbUtils.CreateSqlServerConnection(
                server: SqlServerHost,
                username: SqlServerUsername,
                password: SqlServerPassword,
                useWindowsAuthentication: windowsAuth,
                noDB: true);

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed with error: " + ex.Message);
                return;
            }

            MessageBox.Show("Connection succeeded.");
            connection.Close();
        }
    }
}