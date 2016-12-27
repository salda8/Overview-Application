namespace OverviewApp.Auxiliary
{
    /// <summary>
    ///     A class that holds a status message. Used in StatusSeter
    /// </summary>
    public class StatusMessage
    {
        #region

        public StatusMessage(string status)
        {
            NewStatus = status;
        }

        #endregion

        #region Properties

        public string NewStatus { get; set; }

        #endregion
    }
}