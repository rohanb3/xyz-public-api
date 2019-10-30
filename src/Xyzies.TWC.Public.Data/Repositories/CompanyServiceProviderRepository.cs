using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class CompanyServiceProviderRepository : EfCoreBaseRepository<int, CompanyServiceProvider>, ICompanyServiceProviderRepository
    {
        public CompanyServiceProviderRepository(AppDataContext dbContext) : base(dbContext)
        {

        }
    }
}