using graphApiService.Common;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace graphApiService.Models.User
{
    public class AzureUser
    {
        public string CreationType { get; set; }
        public List<SignInName> SignInNames { get; set; }
        public PasswordProfile PasswordProfile { get; set; }
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        [JsonProperty(Const.RolePropertyName)]
        public string Role { get; set; }
        public string AvatarUrl { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string MailNickname { get; set; }
        public string UsageLocation { get; set; }
        public bool? AccountEnabled { get; set; }
    }
}
