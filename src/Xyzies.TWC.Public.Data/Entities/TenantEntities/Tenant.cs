using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.TenantEntities
{
    public class Tenant : BaseEntity<Guid>
    {
        public Tenant()
        {
            Companies = new List<CompanyTenant>();
        }

        [Required]
        public string TenantName { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual TenantSetting TenantSetting { get; set; }
        public virtual ICollection<CompanyTenant> Companies { get; set; }

    }

}
