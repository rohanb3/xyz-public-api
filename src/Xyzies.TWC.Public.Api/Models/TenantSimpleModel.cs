using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xyzies.TWC.Public.Api.Models
{
    public class TenantWithCompaniesSimpleModel : TenantSingleModel
    {
        public IEnumerable<CompanyBaseModel> Companies { get; set; }
    }
}
