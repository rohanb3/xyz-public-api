using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Xyzies.TWC.DisputeService.Data.Entity.Order
{
    [Table("OfferGroup")]
    public class OfferGroup : BaseEntity<Guid>
    {
        //[Column("OGroupKey")]
        [NotMapped]
        public Guid? OfferGroupKey { get; set; }

        [Column("OGroupKey")]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        [Column("OGroupName")]
        public string GroupName { get; set; }

        [Column("DisplayName")]
        public string DisplayName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? CarrierKey { get; set; }
    }
}
