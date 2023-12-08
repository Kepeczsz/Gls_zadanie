using Newtonsoft.Json;

namespace Gls_Etykiety.Models;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    [JsonProperty("username")]
    public string username { get; private set; } = default!;
    [JsonProperty("password")]
    public string password { get; private set; } = default!;
}
