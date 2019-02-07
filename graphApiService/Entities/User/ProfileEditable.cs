using System.ComponentModel.DataAnnotations;
using graphApiService.Common;
using Newtonsoft.Json;

namespace graphApiService.Entities.User
{
    public class ProfileEditable
    {
        public bool? AccountEnabled { get; set; }
        public int? CompanyId { get; set; }
        public int? RetailerId { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string AvatarUrl { get; set; }
        [StringLength(64)]
        public string GivenName { get; set; }
        [StringLength(64)]
        public string Surname { get; set; }
        [StringLength(64)]
        public string DisplayName { get; set; }
        [StringLength(64)]
        public string City { get; set; }
        [StringLength(64)]
        public string CompanyName { get; set; }
    }
}
