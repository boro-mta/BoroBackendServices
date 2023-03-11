namespace ItemService.API.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string? itemId)
        : base($"entry with id: [{itemId}] already exists")
    {
        
    }
}
