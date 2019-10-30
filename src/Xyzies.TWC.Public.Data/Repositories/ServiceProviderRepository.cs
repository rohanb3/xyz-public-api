using System;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class ServiceProviderRepository : EfCoreBaseRepository<Guid, ServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(AppDataContext dbContext) : base(dbContext)
        {

        }
    }
}