﻿using DataAccess;
using DataStructures;
using OverviewApp.Properties;

namespace OverviewApp.Auxiliary
{
    public static class DataStorageFactory
    {
        public static IDataService Get()
        {
            switch (Settings.Default.DatabaseType)
            {
                case "MySql":
                    return new MySQLStorage();

                case "SqlServer":
                    return new SqlServerStorage();

                default:
                    return new MySQLStorage();
            }
        }
    }
}