namespace ItemService.Controller.Validators;

internal static class InputValidators
{
    internal static bool AreValidGuids(this IEnumerable<string> ids, out IEnumerable<Guid> guids)
    {
        try
        {
            guids = ids.Select(Guid.Parse);
            return true;
        }
        catch (Exception)
        {
            guids = Enumerable.Empty<Guid>();
            return false;
        }
    }
}
