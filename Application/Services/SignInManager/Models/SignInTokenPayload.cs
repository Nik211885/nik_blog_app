using System.Text.Json.Serialization;

namespace Application.Services.SignInManager.Models;

public class SignInTokenPayload
{
    public Guid UserId { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SignInTokenType SignInTokenType { get; init; }
    public DateTimeOffset TokenExpired { get; init; }
}
