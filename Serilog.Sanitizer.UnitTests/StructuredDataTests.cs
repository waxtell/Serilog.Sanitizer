using System.Collections.Generic;
using System.Linq;
using Serilog.Sanitizer.Extensions;
using Serilog.Sanitizer.UnitTests.Sinks;
using Serilog.Events;
using NUnit.Framework;

namespace Serilog.Sanitizer.UnitTests
{
    [TestFixture]
    public class StructuredDataTests
    {
        private const string OverrideValue = "***** VALUE OVERRIDDEN *****";

        internal class Employee
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Ssn { get; set; }
        }

        [Test]
        public void StructuredValueIsRemoved()
        {
            LogEvent? evt = null;

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Structured()
                                    .ByRemoving<Employee>(x => x.Ssn)
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            var employee = new Employee { FirstName = "Monty", LastName = "Python", Ssn = "1234567890" };
            logger.Information("Employee information {@employee}", employee);

            Assert.IsFalse(evt?.Properties.ContainsKey("Ssn"));
        }

        [Test]
        public void StructuredValueIsOverridden()
        {
            LogEvent? evt = null;

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Structured()
                                    .ByOverriding<Employee>((x => x.Ssn, OverrideValue))
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            var employee = new Employee { FirstName = "Monty", LastName = "Python", Ssn = "1234567890" };
            logger.Information("Employee information {@employee}", employee);

            var properties = (evt?.Properties["employee"] as StructureValue)
                                ?.Properties
                                .ToDictionary(p => p.Name, p => p.Value)
                                ?? new Dictionary<string, LogEventPropertyValue>();

            Assert.AreEqual(OverrideValue, (properties["Ssn"] as ScalarValue)?.Value);
        }
    }
}
