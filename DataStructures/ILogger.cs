using System;
using DataStructures.Enums;

namespace DataStructures
{
    public interface ILogger
    {
        /// <summary>
        /// Logs exception.
        /// </summary>
        /// <param name="type">The type of log.</param>
        /// <param name="message">The log message.</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="severity">The log severity.</param>
        void Log(LogType type, string message, Exception e, LogSeverity severity = LogSeverity.Error);

        /// <summary>
        /// Logs the specified event.
        /// </summary>
        /// <param name="type">The type of log.</param>
        /// <param name="message">The log message.</param>
        /// <param name="severity">The log severity.</param>
        void Log(LogType type, string message, LogSeverity severity = LogSeverity.Info);
    }
}