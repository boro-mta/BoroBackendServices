using ItemService.API.Models.Input;

namespace ItemService.Controller.Validators;

internal static class ItemImageInputValidatorExtensions
{
    public static bool IsValidInput(this ItemImageInput input, out IEnumerable<string> errors)
    {
        errors = Enumerable.Empty<string>();
        return true;
    }
}
