using System;
using System.Configuration;
using System.Data.SqlClient;
using OverviewApp.Properties;
using OverviewApp.Views;

namespace OverviewApp.Helpers
{
    public static class DBUtils
    {
        public static void SetConnectionString()
        {
            SetSqlServerConnectionString(Settings.Default.allPurposeDatabaseName);
            SetSqlServerConnectionString(Settings.Default.dataDatabaseName);

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private static void SetSqlServerConnectionString(string dbName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringSettings conSettings = config.ConnectionStrings.ConnectionStrings[dbName];
           

            conSettings.ConnectionString = GetSqlServerConnectionString(
                dbName,
                Settings.Default.sqlServerHost,
                Settings.Default.sqlServerUsername,
                EncryptionUtils.Unprotect(Settings.Default.sqlServerPassword),
                useWindowsAuthentication: Settings.Default.sqlServerUseWindowsAuthentication);
            conSettings.ProviderName = "System.Data.SqlClient";

            config.Save();
        }

        internal static string GetConnectionStringFromAppConfig(string dbName)
        {
            return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        }

        public static SqlConnection CreateSqlServerConnection(string database = "qdms", string server = null, string username = null, string password = null, bool noDB = false, bool useWindowsAuthentication = true)
        {
            return new SqlConnection(GetSqlServerConnectionString(database, server, username, password, noDB, useWindowsAuthentication));
          
        }

        internal static string GetSqlServerConnectionString(string database = "qdms", string server = null, string username = null, string password = null, bool noDB = false, bool useWindowsAuthentication = true)
        {
            string connectionString = $"Data Source={server ?? Settings.Default.sqlServerHost};";

            if (!noDB)
            {
                connectionString += $"Initial Catalog={database};";
            }

            if (!useWindowsAuthentication) //user/pass authentication
            {
                if (password == null)
                {
                    try
                    {
                        password = EncryptionUtils.Unprotect(Settings.Default.sqlServerPassword);
                    }
                    catch
                    {
                        password = "";
                    }
                }
                connectionString += $"User ID={username};Password={password};";
            }
            else //windows authentication
            {
                connectionString += "Integrated Security=True;";
            }
            return connectionString;
        }

        public static void CheckDBConnection(string databaseName="")
        {
            
            SqlConnection connection = CreateSqlServerConnection(noDB: true, useWindowsAuthentication: Settings.Default.sqlServerUseWindowsAuthentication);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                var dbDetailsWindow = new DBConnection_View();
                dbDetailsWindow.ShowDialog();
            }
            connection.Close();
        }
    }
}