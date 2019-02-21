using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task<IQueryable<BranchContact>> GetAsync() =>
            await Task.FromResult(base.Data
                .AsQueryable());
    }
}

