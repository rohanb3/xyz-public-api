using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Table("TWC_Branches")]
    public class Branch : BaseEntity<Guid>
    {
        [Column ("BranchID")]
        public int Id { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public int? Status { get; set; }
        public string State { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        
        public virtual ICollection<BranchContact> BranchContacts { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}