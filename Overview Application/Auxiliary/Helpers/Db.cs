using System;
using MySql.Data.MySqlClient;

namespace OverviewApp.Auxiliary.Helpers
{
    public class Db
    {
        #region Fields

        public static string ConnectionString = @"server=localhost;userid=salda;password=saldik;database=data";

        #endregion

        public static MySqlConnection OpenConnection()
        {
            var connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return connection;
        }
    }
}