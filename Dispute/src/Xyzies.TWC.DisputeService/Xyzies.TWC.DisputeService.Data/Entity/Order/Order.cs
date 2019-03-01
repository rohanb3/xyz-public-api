using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xyzies.TWC.DisputeService.Data.Entity.Order
{
    [Table("TWC_Order")]
    public class Order : BaseEntity<Guid>
    {
        private string _accountNumber = null;

        [Column("OrderId")]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// Offer Group Key/Id
        /// </summary>
        [Column("OGroupKey")]
        public Guid? OfferGroupId { get; set; }

        [Column("AccountNumber")]
        public string AccountNumber
        {
            get => string.IsNullOrWhiteSpace(_accountNumber) ? null : _accountNumber.Trim();
            set => _accountNumber = value;
        }

        /// <summary>
        /// Order Sequence
        /// </summary>
        [Column("Seq")]
        public int Sequence { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column("FirstName")]
        public string CustomerFirstName { get; set; }

        [Column("LastName")]
        public string CustomerLastName { get; set; }

        [Column("Email")]
        public string CustomerEmail { get; set; }

        [Column("Status")]
        public string OrderStatus { get; set; }

        #region Helpers

        [NotMapped]
        public int? AgeDays => CreatedOn.HasValue ? 
            (int?)DateTime.Now.Subtract(CreatedOn.Value).TotalDays :
            null;

        #endregion
    }
}
