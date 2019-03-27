using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    /// <summary>
    /// See BranchContactConfiguration for property details
    /// </summary>
    public class BranchContact : BaseEntity<Guid>
    {
        [Column("BranchContactID")]
        public override Guid Id { get; set; }

        public string PersonName { get; set; }

        public string PersonLastName { get; set; }

        public string PersonTitle { get; set; }

        public string Value { get; set; }

        public DateTime? CreatedDate { get; private set; }

        public DateTime? ModifiedDate { get; private set; }

        public Guid? BranchContactTypeId { get; set; }

        public virtual BranchContactType BranchContactType { get; set; }

        public Guid? BranchPrimaryContactId { get; set; }

        public virtual Branch BranchPrimaryContact { get; set; }
    }
}
