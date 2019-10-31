using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class ServiceProviderRepository : EfCoreBaseRepository<Guid, ServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(AppDataContext dbContext) : base(dbContext)
        {

        }

        public override async Task<IQueryable<ServiceProvider>> GetAsync() => await Task.FromResult(
            base.Data.Include(x => x.Companies));

        public async Task<ServiceProvider> GetServiceProviderByCompany(int companyId) => await Task.FromResult(
            base.Data.FirstOrDefault(x => x.Companies.Select(c => c.CompanyId).Contains(companyId)));
    }
}