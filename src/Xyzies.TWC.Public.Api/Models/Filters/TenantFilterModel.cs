using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xyzies.TWC.Public.Api.Models.Filters
{
    public class TenantFilterModel
    {
        public TenantFilterModel()
        {
            TenantIds = new List<Guid>();
        }

        /// <summary>
        /// Filter by tenant id (GUID)
        /// </summary>
        public IEnumerable<Guid> TenantIds { get; set; }
    }
}
