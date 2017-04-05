using System;
using System.Reactive.Subjects;
using NLog;
using NLog.Targets;

namespace OverviewApp
{
    [Target("MemoryTarget")]
    class MemoryTarget : TargetWithLayout
    {
        private readonly Subject<LogEventInfo> messages = new Subject<LogEventInfo>();
        public IObservable<LogEventInfo> Messages { get; private set; }

        public MemoryTarget()
        {
            Messages = messages;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            messages.OnNext(logEvent);
        }
    }
}