using MS = Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Boro.Validations;

[System.AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
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
        var actionName = context.ActionDescriptor.DisplayName;

        const string entryMessage = "[{type}] - [{context}] - Will attempt to validate argument: [{parameterName}]";
        _LOGGER.LogInformation(entryMessage,
                               TypeName,
                               actionName,
                               ParameterName);

        var propertyPath = ParameterName.Split(".");
        var parameterName = propertyPath.First();
        var parameter = context.ActionArguments[ParameterName];

        if (propertyPath.Length > 1)
        {
            Stack<string> stack = new(propertyPath.Reverse());
            while (stack.TryPop(out string? component))
            {
                var propertyInfo = parameter?.GetType()?.GetProperty(component)
                    ?? throw new ArgumentException($"Property '{component}' not found on object of type '{parameter?.GetType()?.FullName}'");

                parameter = propertyInfo.GetValue(parameter);
            }
        }

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
        else
        {
            const string successMessage = "[{type}] - [{context}] - argument: [{parameterName}] is valid.";
            _LOGGER.LogInformation(successMessage,
                                   TypeName,
                                   actionName,
                                   ParameterName);
        }
    }
}