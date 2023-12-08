using Newtonsoft.Json;

namespace Gls_Etykiety.Models.JsonResponses;

internal class SessionResponse
{
    [JsonProperty("return")]
    public Session SessionData { get; set; } = default!;
}
