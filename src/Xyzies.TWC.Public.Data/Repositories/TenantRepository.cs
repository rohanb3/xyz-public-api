using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities.Tenant;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class TenantRepository : EfCoreBaseRepository<Guid, Tenant>, ITenantRepository
    {
        public TenantRepository(AppDataContext dbContext) : base(dbContext)
        {

        }

        public override async Task<IQueryable<Tenant>> GetAsync() => await Task.FromResult(
            base.Data.Include(x => x.Companies));

        public async Task<Tenant> GetTenantByCompany(int companyId) => await Task.FromResult(
            base.Data.FirstOrDefault(x => x.Companies.Select(c => c.CompanyId).Contains(companyId)));
    }
}