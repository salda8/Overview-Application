using Splat;

namespace OverviewApp
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