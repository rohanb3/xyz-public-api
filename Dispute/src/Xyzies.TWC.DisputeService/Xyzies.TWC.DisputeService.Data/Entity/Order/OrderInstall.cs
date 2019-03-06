using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xyzies.TWC.DisputeService.Data.Entity.Order
{
    [Table("SpectrumAccount")]
    public class OrderInstall : BaseEntity<string>
    {
        #region Properties

        /// <summary>
        /// Account Number
        /// </summary>
        [Key, Column("Account")]
        public override string Id { get => base.Id; set => base.Id = value; }

        [Key, Column("OfferName")]
        public string OfferName { get; set; }

        public string OfferCode { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? InstallOn { get; set; }

        public DateTime? DisconnectOn { get; set; }

        [Column("TppCommission")]
        public decimal Commission { get; set; }

        public Guid? ItemKey { get; set; }

        [Column("ItemSeq")]
        public int ItemSequence { get; set; }

        public int? StatusId { get; set; }

        #endregion

        [NotMapped]
        public string AccountNumber { get => Id; }
    }
}
