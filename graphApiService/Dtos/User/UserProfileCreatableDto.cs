using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace graphApiService.Dtos.User
{
    public class UserProfileCreatableDto
    {
        [Required]
        public string DisplayName { get; set; }
        [DefaultValue(true)]
        public bool AccountEnabled { get; set; }
        [DefaultValue("LocalAccount")]
        public string CreationType { get; set; }
        [StringLength(32)]
        public string MailNickname { get; set; }
        [Required(ErrorMessage = "Password profile is required")]
        public PasswordProfile PasswordProfile { get; set; }
        [StringLength(32)]
        public string UsageLocation { get; set; }
        [Required(ErrorMessage = "There must be at least one SignIn name")]
        public List<SignInName> SignInNames { get; set; }
    }
}
