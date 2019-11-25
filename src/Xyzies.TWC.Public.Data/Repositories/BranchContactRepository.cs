using System;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class BranchContactRepository : EfCoreBaseRepository<Guid, BranchContact>, IBranchContactRepository
    {
        public BranchContactRepository(CablePortalAppDataContext dbContext)
            : base(dbContext)
        {

        }
    }
}

