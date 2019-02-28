using System;
using System.Collections.Generic;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// See BranchConfiguration for property details
    /// </summary>
    public class Branch : BaseEntity<int>
    {
        public string BranchName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string GeoLat { get; set; }
        public string GeoLng { get; set; }

        // TODO: Need to discuss branch statuses
        public BranchStatus Status { get; set; }

        public DateTime? CreatedDate { get; private set; }
        public DateTime? ModifiedDate { get; private set; }

        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual IEnumerable<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}