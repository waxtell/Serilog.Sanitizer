# Serilog.Sanitizer
Log sanitizer for Serilog that supports structured and unstructured log event data.

[![Build](https://github.com/waxtell/Serilog.Sanitizer/actions/workflows/build.yml/badge.svg)](https://github.com/waxtell/Serilog.Sanitizer/actions/workflows/build.yml)

Sanitize (either remove or override) both structured and unstructured data

```csharp
var logger = new LoggerConfiguration()
                .Sanitize()
                    .Structured()
                        .ByOverriding<Employee>((dm => dm.HomeAddress, @"¯\_(ツ)_/¯"))
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

Or, configure the destructured behavior via web/app.config

```csharp
var logger = new LoggerConfiguration()
                .Sanitize()
                    .Unstructured()
                        .FromConfig((ISanitizerConfiguration) ConfigurationManager.GetSection("sanitizer"))
                    .Continue()
                // Configure sinks, etc.
                .CreateLogger();

  <configSections>
    <section name="sanitizer" type="Serilog.Sanitizer.Configuration.SanitizerConfigSection,Serilog.Sanitizer"/>
  </configSections>

  <sanitizer>
    <ToOverride>
      <property Name="Request.Params[HTTP_AUTHORIZATION]" Override="¯\_(ツ)_/¯"/>
      <property Name="Request.Headers[Authorization]" Override="¯\_(ツ)_/¯"/>
    </ToOverride>
    <ToRemove>
      <property Name="Request.Params[ALL_RAW]"/>
      <property Name="Request.Params[ALL_HTTP]"/>
    </ToRemove>
  </sanitizer>
```
