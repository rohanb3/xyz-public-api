using graphApiService.Common;
using Newtonsoft.Json;

namespace graphApiService.Entities.User
{
    public class AzureUser
    {
        public string ObjectId { get; set; }
        public string GivenName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        [JsonProperty(Const.RolePropertyName)]
        public string Role { get; set; }
        public string AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
        public bool? AccountEnabled { get; set; }
    }
}
