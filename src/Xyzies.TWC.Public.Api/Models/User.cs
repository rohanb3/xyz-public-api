using System;
using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api.Models
{
    /// <summary>
    /// User profile information model
    /// </summary>
    public class User
    {
        [Required]
        public string ObjectId { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string CompanyName { get; set; }

        public int? CPUserId { get; set; }

        [StringLength(64)]
        public string DisplayName { get; set; }

        [StringLength(64)]
        public string GivenName { get; set; }

        [StringLength(64)]
        public string Surname { get; set; }

        [StringLength(64)]
        public string City { get; set; }

        public string State { get; set; }

        public virtual int? CompanyId { get; set; }

        public int? RetailerId { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string Status { get; set; }

        public string Role { get; set; }

        public string AvatarUrl { get; set; }

        public Guid? BranchId { get; set; }

        public Guid? DepartmentId { get; set; }

        public bool? AccountEnabled { get; set; }

        [StringLength(32)]
        public string MailNickname { get; set; }

        [StringLength(32)]
        public string UsageLocation { get; set; }
    }
}
