using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    [Table("RequestStatus")]
    public class RequestStatus : BaseEntity<Guid>
    {
        [Column("StatusKey")]
        public override Guid Id { get; set; }
        [Column("StatusName")]
        public string Name { get; set; }
        public string Reason { get; set; }
        public string ProcedureName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid WfKey { get; set; }
        public int? DisplaySeq { get; set; }
    }
}
