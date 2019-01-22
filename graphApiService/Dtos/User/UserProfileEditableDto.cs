using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace graphApiService.Dtos.User
{
    public class UserProfileEditableDto
    {
        [DefaultValue(true)]
        public bool AccountEnabled { get; set; }
        [StringLength(64)]
        public string DisplayName { get; set; }
        [StringLength(64)]
        public string City { get; set; }
        [StringLength(64)]
        public string CompanyName { get; set; }
    }
}
