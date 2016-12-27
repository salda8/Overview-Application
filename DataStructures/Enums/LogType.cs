namespace DataStructures.Enums
{
    public enum LogType : byte
    {
        /// <summary>
        /// Logs to system log
        /// </summary>
        System,
        /// <summary>
        /// Logs to audit log
        /// </summary>
        Audit,
        /// <summary>
        /// Logs to admin log
        /// </summary>
        Admin
    }
}