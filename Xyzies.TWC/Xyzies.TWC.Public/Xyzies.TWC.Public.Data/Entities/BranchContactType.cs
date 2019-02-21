using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    [Table("TWC_BranchContactType")]
    public class BranchContactType : BaseEntity<int>
    {
        [Column("BranchContactTypeID")]
        public new int Id { get; set;}
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
}