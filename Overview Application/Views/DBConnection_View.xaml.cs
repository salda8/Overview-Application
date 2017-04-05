using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OverviewApp.Auxiliary.Helpers;

namespace OverviewApp.Views
{
    /// <summary>
    /// Interaction logic for DBConnection_View.xaml
    /// </summary>
    public partial class DBConnection_View : Window
    {
        public DBConnection_View()
        {
            InitializeComponent();
            
            WindowsAuthenticationRadioBtn.IsChecked = Properties.Settings.Default.sqlServerUseWindowsAuthentication;
            SqlServerAuthenticationRadioBtn.IsChecked = !Properties.Settings.Default.sqlServerUseWindowsAuthentication;

            SqlServerHostTextBox.Text = Properties.Settings.Default.sqlServerHost;
            SqlServerUsernameTextBox.Text =
                string.IsNullOrEmpty(Properties.Settings.Default.sqlServerUsername)
                    ? Properties.Settings.Default.sqlServerUsername
                    : "localhost\\SQLEXPRESS";
            SqlServerPasswordTextBox.Password = "asdf";
        }

        private void SqlServerOKBtn_Click(object sender, RoutedEventArgs e)
        {
            bool windowsAuth = WindowsAuthenticationRadioBtn.IsChecked ?? true;
            SqlConnection connection = DBUtils.CreateSqlServerConnection(
                server: SqlServerHostTextBox.Text,
                username: SqlServerUsernameTextBox.Text,
                password: SqlServerPasswordTextBox.Password,
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

            if (WindowsAuthenticationRadioBtn.IsChecked != null)
            {
                Properties.Settings.Default.sqlServerUseWindowsAuthentication = WindowsAuthenticationRadioBtn.IsChecked.Value;
            }
            Properties.Settings.Default.sqlServerHost = SqlServerHostTextBox.Text;
            Properties.Settings.Default.sqlServerUsername = SqlServerUsernameTextBox.Text;
            Properties.Settings.Default.sqlServerPassword = EncryptionUtils.Protect(SqlServerPasswordTextBox.Password);
            Properties.Settings.Default.databaseType = "SqlServer";
            Properties.Settings.Default.Save();
            
            Close();
        }

        private void SqlServerTestConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            bool windowsAuth = WindowsAuthenticationRadioBtn.IsChecked ?? true;

            SqlConnection connection = DBUtils.CreateSqlServerConnection(
                server: SqlServerHostTextBox.Text,
                username: SqlServerUsernameTextBox.Text,
                password: SqlServerPasswordTextBox.Password,
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
