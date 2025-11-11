using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Application.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LoginProviderEx
{
    Google,
}
