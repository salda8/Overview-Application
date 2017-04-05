using Splat;

namespace OverviewApp.Logger
{
    /// <summary>
    /// Logger implementation
    /// </summary>
    public class Logger : IEnableLogger
    {
        public void Write(string message, LogLevel logLevel)
        {
        }

        public LogLevel Level { get; set; }
    }
}