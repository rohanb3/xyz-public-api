using System;
using System.Collections.Generic;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Models
{
    public class ServiceProviderModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public List<CompanyModel> Companies { get; set; }
    }
}