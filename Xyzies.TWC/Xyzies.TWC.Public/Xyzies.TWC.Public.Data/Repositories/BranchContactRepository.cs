using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class BranchContactRepository : EfCoreBaseRepository<int, BranchContact>, IBranchContactRepository
    {
        public BranchContactRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }
    }
}

