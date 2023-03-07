using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Templates;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Boro.Logging.DependencyInjection;
public static class DependencyInjection
{
    private const string EXPRESSION_FORMAT_OUTPUT_TEMPLATE = "{@t:yyyy-MM-dd HH:mm:ss.fff} {SourceContext} [{@l:u4}]{#if RequestId is not null} [{RequestId}]{#end} {@m:lj} {NewLine}{Exception}";
    private static readonly Dictionary<string, string> _logCategories = new(StringComparer.OrdinalIgnoreCase);
    public static WebApplicationBuilder AddBoroLogging(this WebApplicationBuilder builder, string logsDirectory)
    {
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
                                                                   path: Path.Combine(logsDirectory, categoryName, $"log-{categoryName}-Information-.log"),
                                                                   rollingInterval: RollingInterval.Day,
                                                                   restrictedToMinimumLevel: LogEventLevel.Information);
                                                      writeTo.File(formatter: new ExpressionTemplate(EXPRESSION_FORMAT_OUTPUT_TEMPLATE),
                                                                   path: Path.Combine(logsDirectory, categoryName, $"log-{categoryName}-Warning-.log"),
                                                                   rollingInterval: RollingInterval.Day,
                                                                   restrictedToMinimumLevel: LogEventLevel.Warning);
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
        return app;
    }
}
