using Serilog.Sanitizer.Enrichers;

namespace Serilog.Sanitizer.Vernacular
{
    public class PropertyTypeVernacular
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly StructuredVernacular _structuredVernacular;
        private UnstructuredVernacular _unstructuredVernacular;

        internal PropertyTypeVernacular(LoggerConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration;
            _structuredVernacular = new StructuredVernacular(this);
        }

        internal LoggerConfiguration LoggerConfiguration()
        {
            return _loggerConfiguration;
        }

        public UnstructuredVernacular Unstructured()
        {
            if (_unstructuredVernacular == null)
            {
                var enricher = new SanitizingEnricher();
                
                _loggerConfiguration.Enrich.With(enricher);

                _unstructuredVernacular = new UnstructuredVernacular(this, enricher);
            }

            return _unstructuredVernacular;
        }

        public StructuredVernacular Structured()
        {
            return _structuredVernacular;
        }
    }
}
