using Newtonsoft.Json;

namespace graphApiService.Entities
{

    public class AzureADGraphApiCredentials
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}

