using System;
using System.Linq;
using System.Linq.Expressions;
using Serilog.Sanitizer.Extensions;
using Serilog.Sanitizer.UnitTests.Sinks;
using Serilog.Events;
using NUnit.Framework;

namespace Serilog.Sanitizer.UnitTests
{
    [TestFixture]
    public class SanitizerTests
    {
        internal class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Ssn { get; set; }
        }

        private const string OverrideValue = "***** VALUE OVERRIDDEN *****";

        [Test]
        public void ScalarValueIsOverridden()
        {
            LogEvent evt = null;

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Unstructured()
                                .ByOverriding(("test", OverrideValue))
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            logger.Information("Sensitive Information {@test}", "original value");

            Assert.AreEqual(OverrideValue, ((ScalarValue)evt.Properties["test"]).Value);
        }

        [Test]
        public void StructuredValueIsRemoved()
        {
            LogEvent evt = null;
            Expression<Func<Employee, object>> valueTypeProperty = dm => dm.Ssn;

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Structured()
                                    .ByRemoving(valueTypeProperty)
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            var employee = new Employee { FirstName = "Monty", LastName = "Python", Ssn = "1234567890" };
            logger.Information("Employee information {@employee}", employee);

            Assert.IsFalse(evt.Properties.ContainsKey("Ssn"));
        }

        [Test]
        public void StructuredValueIsOverridden()
        {
            LogEvent evt = null;
            (Expression<Func<Employee, object>>, string) toOverride = (dm => dm.Ssn, OverrideValue);

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Structured()
                                    .ByOverriding<Employee>(toOverride)
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            var employee = new Employee { FirstName = "Monty", LastName = "Python", Ssn = "1234567890" };
            logger.Information("Employee information {@employee}", employee);

            var properties = ((StructureValue)evt.Properties["employee"])
                                .Properties
                                .ToDictionary(p => p.Name, p => p.Value);

            Assert.AreEqual(OverrideValue, ((ScalarValue)properties["Ssn"]).Value);
        }
    }
}
