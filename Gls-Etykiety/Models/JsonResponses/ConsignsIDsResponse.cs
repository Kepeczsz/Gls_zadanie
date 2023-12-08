using Newtonsoft.Json;

namespace Gls_Etykiety.Models.JsonResponses;

public class ConsignsIDsResponse
{
    [JsonProperty("return")]
    public ConsignsIDsArray ConsignsIDsArray { get; set; } = default!;
}
