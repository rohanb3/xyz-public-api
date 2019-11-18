using System;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.TenantEntities
{
    public class TenantSetting : BaseEntity<Guid>
    {
        public string Settings { get; set; }
        public Guid TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}