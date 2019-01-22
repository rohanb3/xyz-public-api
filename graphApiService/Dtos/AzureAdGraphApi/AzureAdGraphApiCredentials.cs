using Newtonsoft.Json;

namespace graphApiService.Dtos.AzureAdGraphApi
{
    public class AzureAdGraphApiCredentials
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}

