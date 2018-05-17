using System.Configuration;

namespace Serilog.Sanitizer.Configuration
{
    public class RemoveConfigElement : ConfigurationElement
    {
        [ConfigurationProperty(PropertyNames.Name, IsRequired = true)]
        public string Name => (string)this[PropertyNames.Name];

        private struct PropertyNames
        {
            public const string Name = "Name";
        }
    }
}
