using Newtonsoft.Json;

namespace Auth.API.Models;

public sealed class Auth0ErrorModel
{
    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; }
}
