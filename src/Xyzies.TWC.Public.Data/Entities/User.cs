using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    public class User : BaseEntity<int>
    {
        [Column("UserID")]
        public new int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int? SalesPersonID { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public string Imagename { get; set; }
        public int? UserRefID { get; set; }
        public string LastName { get; set; }
        public bool? Is_Agreement { get; set; }
        public string XyziesId { get; set; }
        public int? ManagedBy { get; set; }
        public bool? Deleted { get; set; }
        public Guid? UserGuid { get; set; }
        public string IPAddressRestriction { get; set; }
        public string SocialMediaAccount { get; set; }
        public string PhotoID { get; set; }
        public DateTime? PasswordExpiryOn { get; set; }
        public string LoginIpAddress { get; set; }
        public bool? IsPhoneVerified { get; set; }
        public bool? IsIdentityUploaded { get; set; }
        public bool? IsEmailVerified { get; set; }
        public bool? IsUserPictureUploaded { get; set; }
        public int? StatusId { get; set; }

        [Column("BranchID")]
        public Guid? BranchId { get; set; }

        [Column("CompanyID")]
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
