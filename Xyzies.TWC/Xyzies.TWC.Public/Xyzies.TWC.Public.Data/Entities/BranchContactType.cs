using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Xyzies.TWC.Public.Data.Entities
{
    [Table("TWC_BranchContactType")]
    public class BranchContactType
    {
        [Column("BranchContactTypeID")]
        public int Id { get; set;}
        public string Name { get; set; }

        public virtual ICollection<BranchContact> BranchContacts { get; set; }
    }
}