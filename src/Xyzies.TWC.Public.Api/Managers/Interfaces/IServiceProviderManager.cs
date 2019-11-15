using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Models.Filters;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface IServiceProviderManager
    {
        Task<Guid> Create(ServiceProviderRequest request);
        Task Update(Guid id, ServiceProviderRequest request);
        Task UpdateByCompanyId(int companyId, ServiceProviderRequest request);
        Task<IEnumerable<ServiceProviderModel>> Get(TenantFilterModel filterModel);
        Task<ServiceProviderModel> GetExtended(Guid id);
        Task<ServiceProviderSingleModel> GetSingle(Guid id);
        Task<ServiceProviderSingleModel> GetSingle(int companyId);
        Task<ServiceProviderModel> Get(int companyId);
    }
}