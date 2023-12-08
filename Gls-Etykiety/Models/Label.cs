using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

public class Label
{
    public Guid Id { get; set; } = default!;

    [JsonProperty("labels")]
    public string Data { get; set; } = default!;

    public Guid UserId { get; set; }
}
