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

    private static (bool valid, IEnumerable<string> errors) ValidateList(IEnumerable<string> l)
    {
        bool allValid = true;
        IEnumerable<string> allErrors = Enumerable.Empty<string>();
        foreach (var s in l)
        {
            var (valid, errors) = ValidateString(s);
            allValid &= valid;
            allErrors = allErrors.Union(errors);
        }

        return (true, Enumerable.Empty<string>());
    }

    public override (bool valid, IEnumerable<string> errors) Validate(object? parameter)
    {
        return parameter switch
        {
            null => (false, new string[] { $"parameter is null" }),
            string s => ValidateString(s),
            IEnumerable<string> l => ValidateList(l),
            Guid or IEnumerable<Guid> => (true, Enumerable.Empty<string>()),
            _ => (false, new string[] { $"parameter {parameter} could not be validated as a Guid or a collection of Guids" })
        };
    }
}