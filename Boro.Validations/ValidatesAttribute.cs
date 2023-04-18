using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MS = Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Boro.Validations;

[System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public abstract class ValidatesAttribute : ActionFilterAttribute
{
    protected static readonly MS.ILogger _LOGGER = MS.LoggerFactory.Create(c => { c.AddSerilog(); }).CreateLogger("Validations");

    public string ParameterName { get; protected set; }
    protected string TypeName { get; init; }
    protected ValidatesAttribute(string parameterName)
    {
        ParameterName = parameterName;
        TypeName = GetType().Name;
    }

    public abstract (bool valid, IEnumerable<string> errors) Validate(object? parameter);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string actionName = context.ActionDescriptor.DisplayName;

        const string entryMessage = "[{type}] - [{context}] - Will attempt to validate argument: [{parameterName}]";
        _LOGGER.LogInformation(entryMessage,
                               TypeName,
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
            _LOGGER.LogError(errorMessage,
                             TypeName,
                             actionName,
                             ParameterName,
                             ParameterName,
                             parameter,
                             errors);

            context.Result = new BadRequestObjectResult($"""
                                Invalid input received.
                                parameter: "{ParameterName}"
                                errors: [
                                {string.Join(Environment.NewLine, errors?.Select(e => $"- {e}") ?? Enumerable.Empty<string>())}
                                ]
                                """);
        }

        const string successMessage = "[{type}] - [{context}] - argument: [{parameterName}] is valid.";
        _LOGGER.LogInformation(successMessage,
                               TypeName,
                               actionName,
                               ParameterName);

    }
}