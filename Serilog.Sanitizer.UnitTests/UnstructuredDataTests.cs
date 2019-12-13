using NUnit.Framework;
using Serilog.Events;
using Serilog.Sanitizer.Extensions;
using Serilog.Sanitizer.UnitTests.Sinks;

namespace Serilog.Sanitizer.UnitTests
{
    [TestFixture]
    public class UnstructuredDataTests
    {
        private const string OverrideValue = "***** VALUE OVERRIDDEN *****";

        [Test]
        public void UnstructuredValueIsOverridden()
        {
            LogEvent evt = null;

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Unstructured()
                                    .ByOverriding(("test", OverrideValue))
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            logger
                .Information
                (
                "Sensitive Information {@test}", 
                "original value"
                );

            Assert
                .AreEqual
                (
                    OverrideValue, 
                    ((ScalarValue) evt.Properties["test"]).Value
                );
        }

        [Test]
        public void UnstructuredValueIsRemoved()
        {
            LogEvent evt = null;

            var logger = new LoggerConfiguration()
                            .Sanitize()
                                .Unstructured()
                                    .ByRemoving("test")
                                .Continue()
                            .WriteTo.Sink(new DelegatingSink(e => evt = e))
                            .CreateLogger();

            logger.Information("Sensitive Information {@test}", "original value");

            Assert.IsFalse(evt.Properties.ContainsKey("test"));
        }
    }
}
