﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    public class BranchContactType : BaseEntity<int>
    {
        [Column("BranchContactTypeID")]
        public new int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
}