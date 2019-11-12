using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.ServiceProvider
{
    public class ServiceProvider : BaseEntity<Guid>
    {
        [Required]
        public string ServiceProviderName { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual List<CompanyServiceProvider> Companies { get; set; }
        public virtual ProviderSetting ProviderSetting { get; set; }

    }
}