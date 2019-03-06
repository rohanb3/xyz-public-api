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

        public override Task<bool> UpdateAsync(Branch entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.ModifiedDate = DateTime.Now;

            return base.UpdateAsync(entity);
        }

        /// <inheritdoc />
        public async Task<bool> SetActivationState(int id, bool isEnabled)
        {
            var branch = await this.GetAsync(id);
            if (branch == null)
            {
                return false;
            }

            branch.IsEnabled = isEnabled;

            return await this.UpdateAsync(branch);
        }

        /// <inheritdoc />
        public override async Task<int> AddAsync(Branch branch)
        {
            branch.CreatedDate = DateTime.Now;
            //var ttt = base.Add(branch);
            return await Task.FromResult(base.Add(branch));
        }
    }
}
