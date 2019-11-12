using System;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.ServiceProvider
{
    public class ProviderSetting : BaseEntity<Guid>
    {
        public string Settings { get; set; }
        public Guid ServiceProviderId { get; set; }

        public virtual ServiceProvider Provider { get; set; }
    }
}