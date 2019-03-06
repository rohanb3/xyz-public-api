using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzies.TWC.Public.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BranchFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public string StateFilter { get; set; }

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
        public string BranchNameFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? BranchIdFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsEnabledFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? UserCountFilter { get; set; }

        /// <summary>
        /// filter for requests from, cancels all other filters
        /// </summary>
        public List<int> UserIds { get; set; } = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetName()
        {
            if (string.IsNullOrEmpty(BranchNameFilter))
            {
                return Enumerable.Empty<string>();
            }

            return BranchNameFilter.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower());
        }
    }
}
