using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface ITenantManager
    {
        Task<Guid> Create(TenantRequest request);
        Task Update(Guid id, TenantRequest request);
        Task UpdateByCompanyId(int companyId, TenantRequest request);
        Task<IEnumerable<TenantModel>> Get();
        Task<TenantModel> GetExtended(Guid id);
        Task<TenantSingleModel> GetSingle(Guid id);
        Task<TenantSingleModel> GetSingle(int companyId);
        Task<TenantModel> Get(int companyId);
    }
}