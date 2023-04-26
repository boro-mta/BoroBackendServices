namespace Boro.Validations;

public class ValidatesGuidsAttribute : ValidatesGuidAttribute
{
    public ValidatesGuidsAttribute(string parameterName) : base(parameterName) { }

    private static (bool valid, IEnumerable<string> errors) ValidateList(IEnumerable<string> list)
    {
        var validations = list.Select(ValidateString).ToArray();
        return (validations.All(v => v.valid), validations.SelectMany(v => v.errors));
    }

    public override (bool valid, IEnumerable<string> errors) Validate(object? parameter) => parameter switch
    {
        null => (false, new string[] { $"parameter is null" }),
        IEnumerable<string> l => ValidateList(l),
        IEnumerable<Guid> => (true, Enumerable.Empty<string>()),
        _ => (false, new string[] { $"parameter {parameter} could not be validated as a collection of Guids" })
    };
}