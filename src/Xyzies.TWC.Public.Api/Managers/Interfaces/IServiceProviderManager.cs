using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface IServiceProviderManager
    {
        Task<Guid> Create(ServiceProviderRequest request);
        Task Update(Guid id, ServiceProviderRequest request);
        Task<IEnumerable<ServiceProviderModel>> Get();
    }
}