using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.Tenant
{
    public class CompanyTenant : BaseEntity<int>
    {
        public int CompanyId { get; set; }
        public Guid TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}