namespace Boro.Validations;

public class ValidatesGuidAttribute : ValidatesAttribute
{
    public ValidatesGuidAttribute(string parameterName) : base(parameterName) { }

    protected static (bool valid, IEnumerable<string> errors) ValidateString(string s)
    {
        if (!Guid.TryParse(s, out _))
        {
            return (false, new string[] { $"a Guid could not be parsed from [{s}]" });
        }
        return (true, Enumerable.Empty<string>());
    }

    public override (bool valid, IEnumerable<string> errors) Validate(object? parameter) => parameter switch
    {
        null => (false, new string[] { $"parameter is null" }),
        string s => ValidateString(s),
        Guid => (true, Enumerable.Empty<string>()),
        _ => (false, new string[] { $"parameter {parameter} could not be validated as a Guid" })
    };
}
