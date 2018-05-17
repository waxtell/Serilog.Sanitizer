using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Sanitizer.Enrichers
{
    internal class SanitizingEnricher : ISanitizingEnricher
    {
        public List<(string propertyName, string overrideValue)> ToOverride { get; set; } = new List<(string propertyName, string overrideValue)>();
        public List<string> ToRemove { get; set; } = new List<string>();

        internal SanitizingEnricher()
        {
        }

        public ISanitizingEnricher Override(params (string propertyName, string overrideValue)[] overrides)
        {
            ToOverride.AddRange(overrides);

            return this;
        }

        public ISanitizingEnricher Remove(params string[] propertyNames)
        {
            ToRemove.AddRange(propertyNames);

            return this;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach(var (propertyName, overrideValue) in ToOverride)
            {
                if (logEvent.Properties.ContainsKey(propertyName))
                {
                    logEvent
                        .AddOrUpdateProperty
                        (
                            new LogEventProperty
                            (
                                propertyName,
                                new ScalarValue(overrideValue)
                            )
                        );
                }
            }

            foreach(var property in ToRemove)
            {
                logEvent.RemovePropertyIfPresent(property);
            }
        }
    }
}
