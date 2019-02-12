using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Xyzies.TWC.DisputeService.Data.Entity
{
    [Table("disputes")]
    public sealed class Dispute : BaseEntity<Guid>
    {
        #region Properties

        public string ExternalDisputeId { get; set; }

        [Required]
        public string AffiliateId { get; set; }

        public string AffiliateName { get; set; }

        public string DisputeType { get; set; }

        public string FiscalPeriod { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        public string WONumber { get; set; }

        public string RetailerPortalConfitmation { get; set; }

        public DateTimeOffset ConnectDate { get; set; }

        public string LegacyCompany { get; set; }

        public string ServiceName { get; set; }

        public string ServiceCode { get; set; }

        public string CustomerName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string CustomerAddress { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string ZIP { get; set; }

        [DataType(DataType.MultilineText)]
        public string SubmitterComment { get; set; }

        // TODO: Attachments

        public DateTimeOffset CreatedAt { get; set; }

        public string DisputeStatus { get; set; }

        public string ClosedStatus { get; set; }

        #endregion

        #region Helpers



        #endregion
    }
}
