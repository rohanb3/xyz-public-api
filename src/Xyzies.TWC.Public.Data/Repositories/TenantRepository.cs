using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public override async Task<IQueryable<Tenant>> GetAsync(Expression<Func<Tenant, bool>> predicate) => await Task.FromResult(
            base.Data.Include(x => x.Companies).Where(predicate));

        public async Task<Tenant> GetTenantByCompany(int companyId) => await Task.FromResult(
            base.Data.FirstOrDefault(x => x.Companies.Select(c => c.CompanyId).Contains(companyId)));
    }
}