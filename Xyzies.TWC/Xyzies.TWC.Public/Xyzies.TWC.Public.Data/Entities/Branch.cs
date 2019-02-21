using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Table("TWC_Branches")]
    public class Branch : BaseEntity<int>
    {
        [Column ("BranchID")]
        public new int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string BranchName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Fax { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string ZipCode { get; set; }
        [MaxLength(50)]
        public string GeoLat { get; set; }
        [MaxLength(50)]
        public string GeoLon { get; set; }
        public int? Status { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        public virtual ICollection<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}