using Application.Entities;

namespace Application.Services.ArgumentManager.Responses;

public class ArgumentResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Query { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
}

public static class ArgumentResponseExtension
{
    public static ArgumentResponse MapToResponse(this Arguments arguments)
    {
        return new ArgumentResponse()
        {
            Id = arguments.Id,
            Code = arguments.Code,
            Description = arguments.Description,
            Query = arguments.Query,
            CreatedAt = arguments.CreatedAt
        };
    }
}