using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Templates;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Configuration;

namespace Boro.Logging;
public static class SerilogConfigurationExtensions
{
    private static readonly Dictionary<string, string> _logCategories = new(StringComparer.OrdinalIgnoreCase);
    private const string EXPRESSION_FORMAT_OUTPUT_TEMPLATE = "{@t:yyyy-MM-dd HH:mm:ss.fff} | {SourceContext} | [{@l:u}] |{#if RequestId is not null} [{RequestId}] |{#end} {@m:lj} {NewLine}{Exception}";
    private static readonly ExpressionTemplate _expressionTemplate = new (EXPRESSION_FORMAT_OUTPUT_TEMPLATE);

    public static WebApplicationBuilder AddBoroLogging(this WebApplicationBuilder builder, string logsDirectory)
    {
        builder.Configuration.GetSection("Logging").GetSection("LogLevel").Bind(_logCategories);

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.Enrich.WithProperty("NewLine", Environment.NewLine);
            configuration.WriteTo.Console(formatter: _expressionTemplate,
                restrictedToMinimumLevel: LogEventLevel.Debug);

            configuration.WriteTo.File(formatter: _expressionTemplate,
                path: Path.Combine(logsDirectory, "log-.log"),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information);

            var configs = _logCategories.Where(kvp => !kvp.Key.Equals("Default", StringComparison.OrdinalIgnoreCase))
                                        .Select(logCategory => configuration.WriteTo.Conditional(logEvent => logEvent.FileWritingCondition(logCategory), config => config.FileWritingAction(logCategory, logsDirectory)))
                                        .ToList();
        });

        builder.Logging
            .ClearProviders()
            .AddSerilog();

        return builder;
    }

    private static bool FileWritingCondition(this LogEvent logEvent, KeyValuePair<string, string> logCategory) 
        => logEvent.Properties.TryGetValue("SourceContext", out var v)
           && v is ScalarValue sv
           && sv.Value is string s
           && logCategory.Key.Equals(value: s, comparisonType: StringComparison.OrdinalIgnoreCase);

    private static void FileWritingAction(this LoggerSinkConfiguration writeTo, KeyValuePair<string, string> logCategory, string logsDirectory)
    {
        var categoryName = logCategory.Key;
        var logEventLevel = Enum.TryParse<LogEventLevel>(value: logCategory.Value, ignoreCase: true, result: out var level) ? 
            level : LogEventLevel.Information;

        writeTo.File(formatter: _expressionTemplate,
                    path: Path.Combine(logsDirectory, categoryName, $"{categoryName}-{logEventLevel}-.log"),
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: logEventLevel);

        writeTo.File(formatter: _expressionTemplate,
                    path: Path.Combine(logsDirectory, categoryName, $"{categoryName}-Error-.log"),
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Error);
    }

    public static WebApplication UseBoroLogging(this WebApplication app)
    {
        //logging middlewares can be added here

        return app;
    }
}
