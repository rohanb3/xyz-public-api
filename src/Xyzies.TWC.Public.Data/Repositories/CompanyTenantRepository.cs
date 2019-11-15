using Xyzies.TWC.Public.Data.Entities.Tenant;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class CompanyTenantRepository : EfCoreBaseRepository<int, CompanyTenant>, ICompanyTenantRepository
    {
        public CompanyTenantRepository(AppDataContext dbContext) : base(dbContext)
        {

        }
    }
}