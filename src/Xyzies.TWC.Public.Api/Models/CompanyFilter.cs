using System;
using System.Collections.Generic;

namespace Xyzies.TWC.Public.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public string StateFilter { get; set; }

        /// <summary>
        /// Show company with all statuses
        /// </summary>
        public IList<string> RequestStatusNames { get; set; }

        /// <summary>
        /// Quick search by company name
        /// </summary>
        public string SearchFilter { get; set; }

        /// <summary>
        /// Start date serching
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Terminated date serching
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CityFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EmailFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> CompanyNameFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CompanyIdFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsEnabledFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? BranchCountFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? UserCountFilter { get; set; }

        /// <summary>
        /// filter for requests from, cancels all other filters
        /// </summary>
        public List<int> UserIds { get; set; } = new List<int>();

        /// <summary>
        /// filter for requests from, cancels all other filters
        /// </summary>
        public List<int> CompanyIds { get; set; } = new List<int>();
    }
}
