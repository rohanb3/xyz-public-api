using Newtonsoft.Json;

namespace graphApiService.Dtos.Login
{
    public class UserCredentialsDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
