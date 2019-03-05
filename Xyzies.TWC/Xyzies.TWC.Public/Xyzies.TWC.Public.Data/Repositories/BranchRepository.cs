using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class BranchRepository : EfCoreBaseRepository<int, Branch>, IBranchRepository
    {
        public BranchRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<Branch> GetAsync(int id)
        {
            var branches = await Data
                .Include(x => x.BranchContacts)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

            return branches;
        }

        /// <inheritdoc />
        public override async Task<IQueryable<Branch>> GetAsync() =>
            await Task.FromResult(base.Data
                .Include(b => b.BranchContacts)
                    .ThenInclude(x => x.BranchContactType)
                .Include(v=>v.BranchUsers));

        /// <inheritdoc />
        public override async Task<IQueryable<Branch>> GetAsync(Expression<Func<Branch, bool>> predicate) =>
            await Task.FromResult(base.Data
                .Include(b => b.BranchContacts)
                    .ThenInclude(x => x.BranchContactType)
                .Where(predicate));

        /// <inheritdoc />
        public async Task<bool> SetActivationState(int id, bool isEnabled)
        {
            var company = await base.Data.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return false;
            }

            company.IsEnabled = isEnabled;

            base.Data.Update(company);
            await DbContext.SaveChangesAsync();

            return true;
        }
    }
}
