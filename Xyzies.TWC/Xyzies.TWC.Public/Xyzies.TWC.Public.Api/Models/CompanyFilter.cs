using System;
using System.Linq;
using System.Collections.Generic;
using Xyzies.TWC.Public.Api.Http.Extentions;

namespace Xyzies.TWC.Public.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyFilter : Searchable
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
        public string CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCompanyName()
        {
            if (string.IsNullOrEmpty(CompanyName))
            {
                return Enumerable.Empty<string>();
            }

            return CompanyName.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower());
        }
    }
}
