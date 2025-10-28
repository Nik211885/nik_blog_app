using System.ComponentModel.DataAnnotations;
using Application.Entities;

namespace Application.Services.ArgumentManager.Requests;

public class UpdateArgumentResponse
{
    public Guid ArgumentId { get; init; }
    [Required(ErrorMessage = ArgumentConstMessage.CodeCanNotBeNull)]
    public string Code { get; init; } = string.Empty;
    [Required(ErrorMessage = ArgumentConstMessage.DescriptionCanNotBeNull)]
    public string Description { get; init; } = string.Empty;
    [Required(ErrorMessage = ArgumentConstMessage.QueryCanNotBeNull)]
    public string Query { get; init; } = string.Empty;
}


internal static class UpdateArgumentRequestExtensions
{
    public static void MapToArgument(this UpdateArgumentResponse request, Arguments arguments)
    {
        arguments.Code = request.Code;
        arguments.Description = request.Description;
        arguments.Query = request.Query;
    }
}