using System;
using System.Collections.Generic;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Branch : BaseEntity<int>
    {
        public new int Id { get; set; }
        public string BranchName { get; set; }
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
        public DateTime? CreatedDate { get; private set; }
        public DateTime? ModifiedDate { get; private set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public int ParentCompanyId { get; set; }
        public virtual Company ParentCompany { get; set; }

        public virtual IEnumerable<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}