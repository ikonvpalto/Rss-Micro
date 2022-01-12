using Newtonsoft.Json;

namespace Auth.Common.Models;

public sealed class UserProfile
{
    [JsonProperty("sub")]
    public string? Sub { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("picture")]
    public string? Picture { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }
}
