# Serilog.Sanitizer
Log sanitizer for Serilog that supports structured and unstructured log event data.

Sanitize (either remove or override) both structured and unstructured data

```csharp
var logger = new LoggerConfiguration()
                .Sanitize()
                    .Structured()
                        .ByOverriding<Employee>((dm => dm.HomeAddress, "****** SENSITIVE INFORMATION ******"))
                        .ByRemoving<Employee>(dm => dm.Ssn)
                    .Unstructured()
                        .ByOverriding(("Request.Params[HTTP_AUTHORIZATION]", @"¯\_(ツ)_/¯"))
                        .ByOverriding(("Request.Headers[Authorization]", @"¯\_(ツ)_/¯"))
                        .ByRemoving("Request.Params[ALL_RAW]")
                        .ByRemoving("Request.Params[ALL_HTTP]")
                    .Continue()
                // Configure sinks, etc.
                .CreateLogger();
```