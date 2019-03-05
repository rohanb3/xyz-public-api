using System;
using System.Linq;
using System.Collections.Generic;

namespace Xyzies.TWC.Public.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyFilter //: Searchable
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
        public string CompanyNameFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyIdFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabledFilter { get; set; } = true;

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
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetName()
        {
            if (string.IsNullOrEmpty(CompanyNameFilter))
            {
                return Enumerable.Empty<string>();
            }

            return CompanyNameFilter.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower());
        }
    }
}
