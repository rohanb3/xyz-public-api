using System;
using System.Linq;
using System.Collections.Generic;

namespace Xyzies.TWC.Public.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Filter //: Searchable
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
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CountValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Enumerable.Empty<string>();
            }

            return Name.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower());
        }
    }
}
