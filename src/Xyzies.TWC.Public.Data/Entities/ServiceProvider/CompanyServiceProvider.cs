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

        [JsonIgnore]
        public virtual ICollection<Company> Companies { get; set; }
    }
}