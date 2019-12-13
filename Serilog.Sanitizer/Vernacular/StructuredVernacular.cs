using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Serilog.Sanitizer.DestructuringPolicies;

namespace Serilog.Sanitizer.Vernacular
{
    public class StructuredVernacular
    {
        private readonly PropertyTypeVernacular _propertyTypeVernacular;
        private readonly Dictionary<Type, object> _policiesByType;

        internal StructuredVernacular(PropertyTypeVernacular propertyTypeVernacular)
        {
            _propertyTypeVernacular = propertyTypeVernacular;
            _policiesByType = new Dictionary<Type, object>();
        }

        public StructuredVernacular ByRemoving<T>(params Expression<Func<T, object>>[] toRemove)
        {
            DestructuringPolicy<T> policyForType;

            if (!_policiesByType.TryGetValue(typeof(T), out var dictionaryItem))
            {
                policyForType = new DestructuringPolicy<T>();

                _policiesByType.Add(typeof(T), policyForType);

                _propertyTypeVernacular
                    .LoggerConfiguration()
                    .Destructure
                    .With(policyForType);
            }
            else
            {
                policyForType = (DestructuringPolicy<T>) Convert.ChangeType(dictionaryItem, typeof(DestructuringPolicy<T>));
            }

            policyForType.ByRemoving(toRemove);

            return this;
        }

        public StructuredVernacular ByOverriding<T>(params (Expression<Func<T, object>> property,string overrideValue)[] toOverride)
        {
            DestructuringPolicy<T> policyForType;

            if (!_policiesByType.TryGetValue(typeof(T), out var dictionaryItem))
            {
                policyForType = new DestructuringPolicy<T>();

                _policiesByType.Add(typeof(T), policyForType);

                _propertyTypeVernacular
                    .LoggerConfiguration()
                    .Destructure
                    .With(policyForType);
            }
            else
            {
                policyForType = (DestructuringPolicy<T>)Convert.ChangeType(dictionaryItem, typeof(DestructuringPolicy<T>));
            }

            policyForType.ByOverriding(toOverride);

            return this;
        }

        public UnstructuredVernacular Unstructured()
        {
            return _propertyTypeVernacular.Unstructured();
        }

        public LoggerConfiguration Continue()
        {
            return _propertyTypeVernacular.LoggerConfiguration();
        }
    }
}
