using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    [Table("TWC_BranchContact")]
    public class BranchContact : BaseEntity<int>
    {
        [Key]
        [Column("BranchContactID")]
        public new int Id { get; set; }
        [MaxLength(50)]
        public string PersonName { get; set; }
        [MaxLength(50)]
        public string PersonLastName { get; set; }
        [MaxLength(100)]
        public string PersonTitle { get; set; }

        [Required]
        [MaxLength(250)]
        public string Value { get; set; }

        [Required]
        public virtual BranchContactType BranchContactType { get; set; }

        [NotMapped]
        public virtual Branch Branch { get; set; }
    }
}
