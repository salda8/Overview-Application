using System;
using System.Reflection;
using DataStructures;
using DataStructures.Enums;
using log4net;
using OverviewApp.Auxiliary.Helpers;
using GalaSoft.MvvmLight.Ioc;

namespace OverviewApp.Logger
{
    /// <summary>
    /// Logger implementation
    /// </summary>
    public class Logger : ILogger
    {
        #region Fields

        private readonly ILog systemLog;
        private readonly ILog auditLog;
        private readonly ILog adminLog;

        #endregion

        #region

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger() : this(new AttributesHelper())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class for unit testing.
        /// </summary>
        [PreferredConstructor]
        public Logger(IAttributesHelper attributesHelper)
        {
            try
            {
                //set replacement of %property{AssemblyName} in lg4dotnet.xml file
                GlobalContext.Properties["AssemblyName"] = Assembly.GetEntryAssembly().GetName().Name;
                //GlobalContext.Properties["CustomerName"] = attributesHelper.GetAtrributeValue<CustomerName>(Assembly.GetEntryAssembly(), attribute => attribute.Customer,
                //    attributesHelper.CustomerNameFallback);
                GlobalContext.Properties["AssemblyProduct"] = attributesHelper.GetAtrributeValue<AssemblyProductAttribute>(Assembly.GetEntryAssembly(), attribute => attribute.Product,
                    attributesHelper.ProductNameFallback);
                systemLog = LogManager.GetLogger("SYSTEM");
                auditLog = LogManager.GetLogger("AUDIT");
                adminLog = LogManager.GetLogger("ADMINISTRATOR");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion


        #region Methods

        #region Public

        /// <summary>
        /// Logs exception.
        /// </summary>
        /// <param name="type">The type of log.</param>
        /// <param name="message">The log message.</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="severity">The log severity.</param>
        public void Log(LogType type, string message, Exception e, LogSeverity severity = LogSeverity.Error)
        {
            try
            {
                LogToLogger(GetLogger(type), message, e, severity);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        /// <summary>
        /// Logs the specified event.
        /// </summary>
        /// <param name="type">The type of log.</param>
        /// <param name="message">The log message.</param>
        /// <param name="severity">The log severity.</param>
        public void Log(LogType type, string message, LogSeverity severity = LogSeverity.Info)
        {
            try
            {
                LogToLogger(GetLogger(type), message, severity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Private

        private ILog GetLogger(LogType type)
        {
            switch (type)
            {
                case LogType.System:
                    return systemLog;
                case LogType.Audit:
                    return auditLog;
                case LogType.Admin:
                    return adminLog;
                default:
                    return systemLog;
            }
        }

        private void LogToLogger(ILog logger, string message, Exception e, LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    logger.Debug(message, e);
                    break;
                case LogSeverity.Info:
                    logger.Info(message, e);
                    break;
                case LogSeverity.Warn:
                    logger.Warn(message, e);
                    break;
                case LogSeverity.Error:
                    logger.Error(message, e);
                    break;
                case LogSeverity.Fatal:
                    logger.Fatal(message, e);
                    break;
            }
        }

        private void LogToLogger(ILog logger, string message, LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    logger.Debug(message);
                    break;
                case LogSeverity.Info:
                    logger.Info(message);
                    break;
                case LogSeverity.Warn:
                    logger.Warn(message);
                    break;
                case LogSeverity.Error:
                    logger.Error(message);
                    break;
                case LogSeverity.Fatal:
                    logger.Fatal(message);
                    break;
            }
        }

        #endregion

        #endregion

    }
}
