using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace graphApiService.Dtos.User
{
    public class UserProfileEditableDto
    {
        public bool? AccountEnabled { get; set; }
        public int? CompanyId { get; set; }
        public int? RetailerId { get; set; }
        [Phone]
        public string Phone { get; set; }
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
