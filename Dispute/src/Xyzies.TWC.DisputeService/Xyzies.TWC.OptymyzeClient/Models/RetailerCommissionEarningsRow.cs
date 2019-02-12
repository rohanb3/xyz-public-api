using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class RetailerCommissionEarningsRow
    {
        public DateTime ClosedDate { get; set; }

        public string WONumber { get; set; }

        public string Account { get; set; }

        public string BundleName { get; set; }

        public int VideoPSU { get; set; }

        public int InternetPSU { get; set; }

        public int PhonePSU { get; set; }

        public decimal BountyRate { get; set; }

        public string Direction { get; set; }
    }
}
