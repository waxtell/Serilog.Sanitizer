using System.Configuration;

namespace Serilog.Sanitizer.Configuration
{
    public class OverrideConfigElement : ConfigurationElement
    {
        [ConfigurationProperty(PropertyNames.Name, IsRequired = true)]
        public string Name => (string)this[PropertyNames.Name];

        [ConfigurationProperty(PropertyNames.Override, IsRequired = true)]
        public string Override => (string)this[PropertyNames.Override];

        private struct PropertyNames
        {
            public const string Name = "Name";
            public const string Override = "Override";
        }
    }
}
