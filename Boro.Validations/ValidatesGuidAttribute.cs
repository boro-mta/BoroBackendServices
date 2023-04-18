namespace Boro.Validations;

public class ValidatesGuidAttribute : ValidatesAttribute
{
    public ValidatesGuidAttribute(string parameterName) : base(parameterName) { }

    private static (bool valid, IEnumerable<string> errors) ValidateString(string s)
    {
        if (!Guid.TryParse(s, out _))
        {
            return (false, new string[] { $"a Guid could not be parsed from [{s}]" });
        }
        return (true, Enumerable.Empty<string>());
    }

    private static (bool valid, IEnumerable<string> errors) ValidateList(IEnumerable<string> list)
    {
        var validations = list.Select(ValidateString).ToArray();
        return (validations.All(v => v.valid), validations.SelectMany(v => v.errors));
    }

    public override (bool valid, IEnumerable<string> errors) Validate(object? parameter) => parameter switch
    {
        null => (false, new string[] { $"parameter is null" }),
        string s => ValidateString(s),
        IEnumerable<string> l => ValidateList(l),
        Guid or IEnumerable<Guid> => (true, Enumerable.Empty<string>()),
        _ => (false, new string[] { $"parameter {parameter} could not be validated as a Guid or a collection of Guids" })
    };
}