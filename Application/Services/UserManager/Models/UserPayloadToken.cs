using System.Text.Json.Serialization;

namespace Application.Services.UserManager.Models;
public class UserPayloadToken
{
    public string UserId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string SecurityStamp { get; init; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserTokenType TokenType { get; init; }
    public DateTimeOffset TokenExpired { get; init; }
}

