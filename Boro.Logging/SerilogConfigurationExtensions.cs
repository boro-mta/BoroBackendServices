using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Templates;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Boro.Logging;
public static class SerilogConfigurationExtensions
{
    private static readonly Dictionary<string, string> _logCategories = new(StringComparer.OrdinalIgnoreCase);
    public static WebApplicationBuilder AddBoroLogging(this WebApplicationBuilder builder, string logsDirectory)
    {
        const string EXPRESSION_FORMAT_OUTPUT_TEMPLATE = "{@t:yyyy-MM-dd HH:mm:ss.fff} | {SourceContext} | [{@l:u}] |{#if RequestId is not null} [{RequestId}] |{#end} {@m:lj} {NewLine}{Exception}";

        builder.Configuration.GetSection("Logging").GetSection("LogLevel").Bind(_logCategories);

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.Enrich.WithProperty("NewLine", Environment.NewLine);
            configuration.WriteTo.Console(formatter: new ExpressionTemplate(EXPRESSION_FORMAT_OUTPUT_TEMPLATE),
                restrictedToMinimumLevel: LogEventLevel.Debug);

            configuration.WriteTo.File(formatter: new ExpressionTemplate(EXPRESSION_FORMAT_OUTPUT_TEMPLATE),
                path: Path.Combine(logsDirectory, "log-.log"),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information);

            foreach (var logCategory in _logCategories.Where(kvp => !kvp.Key.Equals("Default", StringComparison.OrdinalIgnoreCase)))
            {
                var categoryName = logCategory.Key;
                configuration.WriteTo.Conditional(logEvent => logEvent.Properties.TryGetValue("SourceContext", out var v)
                                                              && v is ScalarValue sv
                                                              && sv.Value is string s
                                                              && logCategory.Key.Equals(s.ToString(), comparisonType: StringComparison.OrdinalIgnoreCase),
                                                  writeTo =>
                                                  {
                                                      writeTo.File(formatter: new ExpressionTemplate(EXPRESSION_FORMAT_OUTPUT_TEMPLATE),
                                                                   path: Path.Combine(logsDirectory, categoryName, $"{categoryName}-Information-.log"),
                                                                   rollingInterval: RollingInterval.Day,
                                                                   restrictedToMinimumLevel: LogEventLevel.Information);
                                                      writeTo.File(formatter: new ExpressionTemplate(EXPRESSION_FORMAT_OUTPUT_TEMPLATE),
                                                                   path: Path.Combine(logsDirectory, categoryName, $"{categoryName}-Error-.log"),
                                                                   rollingInterval: RollingInterval.Day,
                                                                   restrictedToMinimumLevel: LogEventLevel.Error);
                                                  });
            }

        });

        builder.Logging
            .ClearProviders()
            .AddSerilog();

        return builder;
    }

    public static WebApplication UseBoroLogging(this WebApplication app)
    {
        //logging middlewares can be added here

        return app;
    }
}
