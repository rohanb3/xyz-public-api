using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface IServiceProviderRepository : IRepository<Guid, ServiceProvider>
    {
        Task<ServiceProvider> GetServiceProviderByCompany(int companyId);
    }
}