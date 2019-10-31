using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.ServiceProvider
{
    public class CompanyServiceProvider : BaseEntity<int>
    {
        public int CompanyId { get; set; }
        public Guid ServiceProviderId { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}