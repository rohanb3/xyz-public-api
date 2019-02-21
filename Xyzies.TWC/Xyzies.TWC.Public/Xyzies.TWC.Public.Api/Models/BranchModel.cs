using System;
using System.Collections.Generic;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BranchModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public string Status { get; set; }
        public string State { set; get; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }


        public virtual ICollection<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
