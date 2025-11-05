using System.Text.Json.Serialization;
using Application.Entities;
using Application.Enums;

namespace Application.Services.SignInManager.Requests;

public class LinkWithProviderRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LoginProviderEx Provider { get; init; }
    public Guid UserId { get; init; }
    public string Identifier { get; init; } = string.Empty;
}

public static class LinkWithProviderRequestExtensions
{
    public static LoginProvider MapToProvider(this LinkWithProviderRequest request)
    {
        return new LoginProvider()
        {
            Provider = request.Provider,
            Identifier = request.Identifier,
            UserId = request.UserId,
        };
    }
    public static LoginProviderEx  ConvertToLoginProviderEx(string provider)
    {
        return provider switch
        {
            "Google" => LoginProviderEx.Google, 
            _ => throw new Exception($"Not support login with provider {provider}")
        };
    }
}