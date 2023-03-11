using ItemService.API.Models;

namespace ItemService.Controller.Validators;

public static class ItemInputValidatorExtensions
{
    public static bool IsValidInput(this ItemInput input, out IEnumerable<string> errors)
    {
        errors = Enumerable.Empty<string>();
        return true;
    }
}
