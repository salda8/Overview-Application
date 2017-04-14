using GalaSoft.MvvmLight.Messaging;

namespace OverviewApp
{
    #region Set Status

    /// <summary>
    ///     Global class used to set the status message. Whatever you set to catch
    ///     this message will be the one changing the status label/textbox.
    ///     This is static, so it can be called from any point in the program. It
    ///     sends a "StatusMessage" class, which is just a wrapper fro a string,
    ///     but you can obviously extend this if needed.
    /// </summary>
    public static class StatusSetter
    {
        public static void SetStatus(string s)
        {
            Messenger.Default.Send(new StatusMessage(s));
        }
    }

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

        #endregion Set Status

        #region Properties

        public string NewStatus { get; set; }

        #endregion Properties
    }

    #endregion

    #region Switch view

    #endregion
}