using Newtonsoft.Json;

namespace graphApiService.Dtos.User
{
    public class UserProfileEditableDto
    {
        [JsonProperty("accountEnabled")]
        public  bool AccountEnabled { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
    }
}
