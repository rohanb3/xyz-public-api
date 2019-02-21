using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class BranchRepository : EfCoreBaseRepository<int, Branch>, IBranchRepository
    {
        public BranchRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<IQueryable<Branch>> GetAsync() =>
            await Task.FromResult(base.Data
                .AsQueryable());

        public override async Task<Branch> GetAsync(int id) =>
            await Data
            .Include(r => r.BranchContacts)
            .FirstOrDefaultAsync<Branch>(entity => entity.Id.Equals(id));

        /// <inheritdoc />
        public override int Add(Branch entity)
        {
            var id = Data.Add(entity).Entity.Id;
            return id;
        }
    }
}
