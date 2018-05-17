using Serilog.Sanitizer.Vernacular;

namespace Serilog.Sanitizer.Extensions
{
    public static class LoggerConfigurationIgnoreExtensions
    {
        public static PropertyTypeVernacular Sanitize(this LoggerConfiguration configuration)
        {
            return new PropertyTypeVernacular(configuration);
        }
    }
}