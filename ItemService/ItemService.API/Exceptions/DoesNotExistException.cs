namespace ItemService.API.Exceptions;

public class DoesNotExistException : Exception
{
    public DoesNotExistException(string? itemId)
        : base($"entry with id: [{itemId}] does not exist")
    {
        
    }
}
