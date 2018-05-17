using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Serilog.Sanitizer.Configuration
{
    class SanitizerConfigSection : ConfigurationSection, ISanitizerConfiguration
    {
        [ConfigurationProperty(PropertyNames.ToOverride, IsRequired = true)]
        [ConfigurationCollection(typeof(OverrideConfigElement), AddItemName = "property", ClearItemsName = "clear", RemoveItemName = "remove")]
        public GenericConfigurationElementCollection<OverrideConfigElement> ToOverride => (GenericConfigurationElementCollection<OverrideConfigElement>)this[PropertyNames.ToOverride];

        public IEnumerable<(string propertyName, string overrideValue)> PropertiesToOverride => ToOverride.AsEnumerable().Select(x => (x.Name, x.Override));

        [ConfigurationProperty(PropertyNames.ToRemove, IsRequired = true)]
        [ConfigurationCollection(typeof(RemoveConfigElement), AddItemName = "property", ClearItemsName = "clear", RemoveItemName = "remove")]
        public GenericConfigurationElementCollection<RemoveConfigElement> ToRemove => (GenericConfigurationElementCollection<RemoveConfigElement>)this[PropertyNames.ToRemove];

        public IEnumerable<string> PropertiesToRemove => ToRemove.AsEnumerable().Select(x => x.Name);

        private struct PropertyNames
        {
            public const string ToOverride = "ToOverride";
            public const string ToRemove = "ToRemove";
        }
    }
}
