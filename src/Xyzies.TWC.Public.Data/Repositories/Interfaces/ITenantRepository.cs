using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface ITenantRepository : IRepository<Guid, Tenant>
    {
        Task<Tenant> GetTenantByCompany(int companyId);
    }
}