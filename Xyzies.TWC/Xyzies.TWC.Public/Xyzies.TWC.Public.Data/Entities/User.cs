using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    public class User : BaseEntity<int>
    {
        [Column("UserID")]
        public new int Id { get; set; }
        public int? CompanyID { get; set; }
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
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
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

        public string IsRegisteredUser { get; set; }
        public string AuthKey { get; set; }
        public string InfusionSoftId { get; set; }
        public Guid? UserStatusKey { get; set; }
        public DateTime? UserStatusChangedOn { get; set; }
        public int? UserStatusChangedBy { get; set; }

        public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
