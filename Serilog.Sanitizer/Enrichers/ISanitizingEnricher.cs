using Serilog.Core;

namespace Serilog.Sanitizer.Enrichers
{
    public interface ISanitizingEnricher : ILogEventEnricher
    {
        ISanitizingEnricher Override(params (string propertyName, string overrideValue)[] overrides);
        ISanitizingEnricher Remove(params string[] propertyNames);
    }
}