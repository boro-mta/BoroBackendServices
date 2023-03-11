using ItemService.API.Models;

namespace ItemService.Controller.Validators;

internal static class ItemInputValidatorExtensions
{
    internal static bool IsValidInput(this ItemInput input, out IEnumerable<string> errors)
    {
        errors = Enumerable.Empty<string>();
        return true;
    }
}
