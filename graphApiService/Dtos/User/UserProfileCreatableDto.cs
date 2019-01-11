using System.Collections.Generic;
using Microsoft.Azure.ActiveDirectory.GraphClient.Internal;

namespace graphApiService.Dtos.User
{
    public class UserProfileCreatableDto
    {

        public string DisplayName { get; set; }
        public bool AccountEnabled { get; set; }
        public string CreationType { get; set; }
        public string MailNickname { get; set; }
        public PasswordProfile PasswordProfile { get; set; }
        public string UsageLocation { get; set; }
        public List<SignInName> SignInNames { get; set; }
    }
}
