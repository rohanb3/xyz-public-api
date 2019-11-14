using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.Tenant
{
    public class Tenant : BaseEntity<Guid>
    {
        [Required]
        public string TenantName { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual List<CompanyTenant> Companies { get; set; }

    }
}