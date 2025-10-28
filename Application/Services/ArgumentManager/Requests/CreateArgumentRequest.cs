using System.ComponentModel.DataAnnotations;
using Application.Entities;

namespace Application.Services.ArgumentManager.Requests;

public class CreateArgumentRequest
{
    [Required(ErrorMessage = ArgumentConstMessage.CodeCanNotBeNull)]
    public string Code { get; init; } = string.Empty;
    [Required(ErrorMessage = ArgumentConstMessage.DescriptionCanNotBeNull)]
    public string Description { get; init; } = string.Empty;
    [Required(ErrorMessage = ArgumentConstMessage.QueryCanNotBeNull)]
    public string Query { get; init; } = string.Empty;
}

internal static class CreateArgumentRequestExtensions
{
    public static Arguments MapToArgument(this  CreateArgumentRequest request)
    {
        return new Arguments()
        {
            Code = request.Code,
            Description = request.Description,
            Query = request.Query,
        };
    }
}