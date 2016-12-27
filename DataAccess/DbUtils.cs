using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public static class DbUtils
    {
        public static void SetConnectionString()
        {
            //if (Settings.Default.DatabaseType == "MySql")
            //{
            //    SetMySqlConnectionString("qdmsEntities", "qdms");
            //    SetMySqlConnectionString("qdmsDataEntities", "qdmsdata");
            //}
            //else
            //{
            //    SetSqlServerConnectionString("qdmsEntities", "qdms");
            //    SetSqlServerConnectionString("qdmsDataEntities", "qdmsdata");
            //}

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private static void SetSqlServerConnectionString(string stringName, string dbName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var conSettings = config.ConnectionStrings.ConnectionStrings[stringName];

            //this is an extremely dirty hack that allows us to change the connection string at runtime
            var fi = typeof (ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(conSettings, false);

            //conSettings.ConnectionString = GetSqlServerConnectionString(
            //    dbName,
            //    Settings.Default.SqlServerHost,
            //    Settings.Default.SqlServerUsername,
            //    EncryptionUtils.Unprotect(Settings.Default.SqlServerPassword),
            //    useWindowsAuthentication: Settings.Default.SqlServerUseWindowsAuthentication);
            conSettings.ProviderName = "System.Data.SqlClient";

            config.Save();
        }

        private static void SetMySqlConnectionString(string stringName, string dbName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var conSettings = config.ConnectionStrings.ConnectionStrings[stringName];

            //this is an extremely dirty hack that allows us to change the connection string at runtime
            //var fi = typeof (ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            //fi.SetValue(conSettings, false);

            //conSettings.ConnectionString =
            //    $"User Id={Settings.Default.MySqlUsername};Password={EncryptionUtils.Unprotect(Settings.Default.MySqlServerPassword)};Host={Settings.Default.MySqlServerHost};Database={dbName};Persist Security Info=True";
            conSettings.ProviderName = "MySql.Data.MySqlClient";

            config.Save();
        }

        public static SqlConnection CreateSqlServerConnection(string database = "qdms", string server = null,
            string username = null, string password = null, bool noDB = false, bool useWindowsAuthentication = true)
        {
            var connectionString = GetSqlServerConnectionString(database, server, username, password, noDB,
                useWindowsAuthentication);
            return new SqlConnection(connectionString);
        }

        private static string GetSqlServerConnectionString(string database, string server,
            string username, string password , bool noDB=false, bool useWindowsAuthentication = true)
        {
            var connectionString = $"Data Source={server};";

            if (!noDB)
            {
                connectionString += $"Initial Catalog={database};";
            }

            if (!useWindowsAuthentication) //user/pass authentication
            {
                
                connectionString += $"User ID={username};Password={password};";
            }
            else //windows authentication
            {
                connectionString += "Integrated Security=True;";
            }
            return connectionString;
        }

        public static MySqlConnection CreateMySqlConnection(string database = "Data", string server = "127.0.0.1",
            string username = "salda", string password = "saldik", bool noDB = false)
        {
            
            var connectionString = $"server={server };" +
                                   $"user id={username };" + $"Password={password};";

            if (!noDB)
            {
                connectionString += $"database={database};";
            }

            connectionString +=
                "allow user variables=true;" +
                "persist security info=true;" +
                "Convert Zero Datetime=True";

            return new MySqlConnection(connectionString);
        }
    }
}