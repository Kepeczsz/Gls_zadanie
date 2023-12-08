using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

internal class SessionResponse
{
    [JsonProperty("return")]
    public Session SessionData { get; set; } = default!;
}
