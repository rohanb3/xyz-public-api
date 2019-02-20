using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    [Table("TWS_BranchContact")]
    public class BranchContact : BaseEntity<Guid>
    {
        [Key]
        [Column("BranchContactID")]
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonTitle { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public virtual BranchContactType BranchContactType { get; set; }
        
        public virtual Branch Branch { get; set; }
    }
}
