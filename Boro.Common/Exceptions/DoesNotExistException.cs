namespace Boro.Common.Exceptions;

public class DoesNotExistException : Exception
{
    public DoesNotExistException(string? Id)
        : base($"entry with id: [{Id}] does not exist")
    {
        this.Id = Id;
    }

    public string? Id { get; }
}
