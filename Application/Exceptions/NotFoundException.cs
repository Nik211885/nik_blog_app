namespace Application.Exceptions;

/// <summary>
///     Define not found process when data is not found
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) 
        : base(message) { }
}