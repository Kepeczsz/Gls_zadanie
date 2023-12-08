using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

public class LabelData
{
    [JsonProperty("labels")]
    public string Data { get; set; } = default!;
}
