using Newtonsoft.Json;

namespace graphApiService.Dtos.User
{
    public class UserProfileEditableDto
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
    }
}
