using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Boro.Validations;

[System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public abstract class ValidatesAttribute : ActionFilterAttribute
{
    protected static readonly Microsoft.Extensions.Logging.ILogger _logger = LoggerFactory.Create(c => { c.AddSerilog(); }).CreateLogger("Validations");

    public string ParameterName { get; protected set; }
    protected ValidatesAttribute(string parameterName)
    {
        ParameterName = parameterName;
    }

    public abstract (bool valid, IEnumerable<string> errors) Validate(object? parameter);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string typeName = GetType().Name;
        string actionName = context.ActionDescriptor.DisplayName;

        const string entryMessage = "[{type}] - [{context}] - Will attempt to validate argument: [{parameterName}]";
        _logger.LogInformation(entryMessage,
                               typeName,
                               actionName,
                               ParameterName);

        var parameter = context.ActionArguments[ParameterName];

        var (valid, errors) = Validate(parameter);

        if (!valid)
        {
            const string errorMessage = """
                    [{type}] - [{context}] - Validation of argument: [{parameterName}] failed.
                    Content of '{parameterName}': {@content}
                    Errors: {@errors}
                    """;
            _logger.LogError(errorMessage,
                             typeName,
                             actionName,
                             ParameterName,
                             ParameterName,
                             parameter,
                             errors);

            context.Result = new BadRequestObjectResult($"""
                            Invalid input received.
                                parameter: "{ParameterName}"
                                errors: [
                                {string.Join(Environment.NewLine, errors.Select(e => $"- {e}"))}
                                ]
                                """);
        }

        const string successMessage = "[{type}] - [{context}] - argument: [{parameterName}] is valid.";
        _logger.LogInformation(successMessage,
                               typeName,
                               actionName,
                               ParameterName);

    }
}