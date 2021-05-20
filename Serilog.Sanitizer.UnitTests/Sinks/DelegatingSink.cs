using Serilog.Core;
using Serilog.Events;
using System;

namespace Serilog.Sanitizer.UnitTests.Sinks
{
    public class DelegatingSink : ILogEventSink
    {
        private readonly Action<LogEvent> _loggingDelegate;

        public DelegatingSink(Action<LogEvent> loggingDelegate)
        {
            _loggingDelegate = loggingDelegate ?? throw new ArgumentNullException(nameof(loggingDelegate));
        }

        public void Emit(LogEvent logEvent)
        {
            _loggingDelegate(logEvent);
        }
    }
}
