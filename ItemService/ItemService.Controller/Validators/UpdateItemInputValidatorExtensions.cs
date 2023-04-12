using ItemService.API.Models.Input;

namespace ItemService.Controller.Validators;

internal static class UpdateItemInputValidatorExtensions
{
    internal static bool IsValidInput(this UpdateItemInput input, out IEnumerable<string> errors)
    {
        errors = Enumerable.Empty<string>();
        return true;
    }
}
