using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.ServiceProvider
{
    public class ServiceProvider : BaseEntity<Guid>
    {
        public ServiceProvider()
        {
            Companies = new List<Company>();
        }

        [Required]
        public string SeviceProviderName { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual List<Company> Companies { get; set; }

    }
}