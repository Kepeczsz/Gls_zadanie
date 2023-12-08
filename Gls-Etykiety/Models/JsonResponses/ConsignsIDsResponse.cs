using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

public class ConsignsIDsResponse
{
    [JsonProperty("return")]
    public ConsignsIDsArray consignsIDsArray { get; set; } = default!;
}
