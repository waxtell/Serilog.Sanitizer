using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Sanitizer.Extensions;
using Serilog.Events;

namespace Serilog.Sanitizer.DestructuringPolicies
{
    internal class DestructuringPolicy<T> : IDestructuringPolicy
    {
        private readonly List<PropertyInfo> _propertiesToInclude;
        private readonly Type _targetType;
        private readonly List<(PropertyInfo propertyInfo, string valueOverride)> _propertiesToOverride;

        public DestructuringPolicy()
        {
            _targetType = typeof(T);
            _propertiesToOverride = new List<(PropertyInfo propertyInfo, string valueOverride)>();

            _propertiesToInclude = _targetType
                                        .GetRuntimeProperties()
                                        .Where(p => p.CanRead)
                                        .ToList();
        }

        public DestructuringPolicy<T> ByRemoving(params Expression<Func<T, object>>[] toRemove)
        {
            var namesOfPropertiesToIgnore = toRemove.Select(x => x.GetPropertyNameFromExpression());

            _propertiesToInclude
                .RemoveAll(x => namesOfPropertiesToIgnore.Contains(x.Name));

            return this;
        }

        public DestructuringPolicy<T> ByOverriding(params (Expression<Func<T, object>> property,string overrideValue)[] toOverride)
        {
            var propertyNames = toOverride.Select(x => x.property.GetPropertyNameFromExpression());

            _propertiesToOverride
                .AddRange
                (
                    from availableProperty in _propertiesToInclude
                    join @override in toOverride on availableProperty.Name equals @override.property.GetPropertyNameFromExpression()
                    select (availableProperty, @override.overrideValue)
                );

            _propertiesToInclude  
                .RemoveAll(x => propertyNames.Contains(x.Name));

            return this;
        }

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            if (value == null || value.GetType() != _targetType)
            {
                result = null;
                return false;
            }

            result = BuildStructure((T)value, propertyValueFactory);

            return true;
        }

        private LogEventPropertyValue BuildStructure(T value, ILogEventPropertyValueFactory propertyValueFactory)
        {
            var structureProperties = new List<LogEventProperty>();

            foreach (var propertyInfo in _propertiesToInclude)
            {
                object propertyValue;

                try
                {
                    propertyValue = propertyInfo.GetValue(value);
                }
                catch (TargetInvocationException ex)
                {
                    SelfLog.WriteLine($"Exception {ex} thrown for property {propertyInfo}", ex);
                    propertyValue = $"Exception {ex.InnerException?.GetType().Name} thrown for property accessor";
                }

                structureProperties
                    .Add
                    (
                        new LogEventProperty
                        (
                            propertyInfo.Name, 
                            BuildLogEventProperty(propertyValue, propertyValueFactory)
                        )
                    );
            }

            foreach (var (propertyInfo, valueOverride) in _propertiesToOverride)
            {
                var logEventPropertyValue = BuildLogEventProperty(valueOverride, propertyValueFactory);
                structureProperties.Add(new LogEventProperty(propertyInfo.Name, logEventPropertyValue));
            }

            return new StructureValue(structureProperties, _targetType.Name);
        }

        private static LogEventPropertyValue BuildLogEventProperty(object propertyValue, ILogEventPropertyValueFactory propertyValueFactory)
        {
            return propertyValue == null
                    ? new ScalarValue(null)
                    : propertyValueFactory.CreatePropertyValue(propertyValue, true);
        }
    }
}
