using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    public class BranchContactType : BaseEntity<Guid>
    {
        [Column("BranchContactTypeID")]
        public override Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}