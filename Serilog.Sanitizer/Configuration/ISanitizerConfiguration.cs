using System.Collections.Generic;

namespace Serilog.Sanitizer.Configuration
{
    public interface ISanitizerConfiguration
    {
        IEnumerable<(string propertyName, string overrideValue)> PropertiesToOverride { get; }
        IEnumerable<string> PropertiesToRemove { get; }
    }
}