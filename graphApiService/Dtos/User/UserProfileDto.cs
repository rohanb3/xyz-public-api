using System.ComponentModel.DataAnnotations;

namespace graphApiService.Dtos.User {
    public class UserProfileDto {
        [Required]
        public string ObjectId { get; set; }
        [Required]
        [StringLength(64)]
        public string DisplayName { get; set; }
        [StringLength(64)]
        public string City { get; set; }
        [StringLength(64)]
        public string CompanyName { get; set; }
        [Required]
        public bool AccountEnabled { get; set; }
    }
}