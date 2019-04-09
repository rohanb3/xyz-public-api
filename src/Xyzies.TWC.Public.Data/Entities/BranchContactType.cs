using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    public class BranchContactType : BaseEntity<Guid>
    {
        [Column("BranchContactTypeID")]
        public override Guid Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
}