using Serilog.Sanitizer.Configuration;
using System.Linq;
using Serilog.Sanitizer.Enrichers;

namespace Serilog.Sanitizer.Vernacular
{
    public class UnstructuredVernacular
    {
        private readonly ISanitizingEnricher _enricher;
        private readonly PropertyTypeVernacular _propertyTypeVernacular;

        internal UnstructuredVernacular(PropertyTypeVernacular propertyTypeVernacular, ISanitizingEnricher enricher)
        {
            _enricher = enricher;
            _propertyTypeVernacular = propertyTypeVernacular;
        }

        public UnstructuredVernacular FromConfig(ISanitizerConfiguration config)
        {
            _enricher.Override(config?.PropertiesToOverride?.ToArray());
            _enricher.Remove(config?.PropertiesToRemove?.ToArray());

            return this;
        }

        public UnstructuredVernacular ByRemoving(params string[] toRemove)
        {
            _enricher.Remove(toRemove);

            return this;
        }
        public UnstructuredVernacular ByOverriding(params (string, string)[] toOverride)
        {
            _enricher.Override(toOverride);

            return this;
        }

        public StructuredVernacular Structured()
        {
            return _propertyTypeVernacular.Structured();
        }

        public LoggerConfiguration Continue()
        {
            return _propertyTypeVernacular.LoggerConfiguration();
        }
    }
}
