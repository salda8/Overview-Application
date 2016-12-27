using OverviewApp.Views;

namespace OverviewApp.Auxiliary
{
    public class DatabaseSettings
    {
        #region

        public DatabaseSettings()
        {
            var databaseConnectionView = new DatabaseConnection_View();
            databaseConnectionView.Show();
        }

        #endregion
    }
}