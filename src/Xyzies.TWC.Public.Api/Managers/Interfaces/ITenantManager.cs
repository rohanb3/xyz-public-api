using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities.Tenant;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface ITenantManager
    {
        Task<Guid> Create(TenantRequest request);
        Task CreateRelation(int companyId, Guid tenantId);
        Task Update(Guid id, TenantRequest request);
        Task UpdateByCompanyId(int companyId, TenantRequest request);
        Task<IEnumerable<TenantModel>> Get();
        Task<TenantModel> GetExtended(Guid id);
        Task<TenantSingleModel> GetSingle(Guid id);
        Task<TenantSingleModel> GetSingle(int companyId);
        Task<TenantModel> Get(int companyId);
    }
}