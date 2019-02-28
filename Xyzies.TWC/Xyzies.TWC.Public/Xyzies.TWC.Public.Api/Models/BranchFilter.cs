using System;
using System.Collections.Generic;
using System.Linq;
using Xyzies.TWC.Public.Api.Http.Extentions;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// Filter criteria of company
    /// </summary>
    public class BranchFilter : Searchable
    {
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetBranchName()
        {
            if (string.IsNullOrEmpty(BranchName))
            {
                return Enumerable.Empty<string>();
            }

            return BranchName.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower());
        }
    }
}
