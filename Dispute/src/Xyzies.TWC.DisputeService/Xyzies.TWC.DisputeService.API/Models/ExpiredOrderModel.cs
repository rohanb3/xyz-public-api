using System;

namespace Xyzies.TWC.DisputeService.API.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    public class ExpiredOrderModel
    {
        /// <summary>
        /// ID of Order
        /// </summary>
        public Guid OrderId { get; set; }

        public int OrderSequence { get; set; }

        public string AccountNumber { get; set; }

        public string ServiceName { get; set; }

        public int Age { get; set; }

        public int UnitsOrdered { get; set; }

        public int UnitsInstalled { get; set; }

        public string CustomerName { get; set; }

        public string OrderStatus { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
