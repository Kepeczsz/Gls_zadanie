using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

public class Label
{
    [JsonProperty("id")]
    public Guid Id { get; set; } = default!;

    [JsonProperty("Data")]
    public string Data { get; set; } = default!;

    public Guid UserId { get; set; }
}
