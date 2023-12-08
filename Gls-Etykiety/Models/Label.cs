using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

public class Label
{
    [JsonProperty("labels")]
    public string Data { get; set; } = default!;

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
