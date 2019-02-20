using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class BranchRepository : EfCoreBaseRepository<Guid, Branch>, 
        //IRepository<Guid, Branch>,
        IBranchRepository
    {
        public BranchRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<IQueryable<Branch>> GetAsync()
        {
            return await Task.FromResult(base.Data.AsQueryable());
        }

        ///// <inheritdoc />
        public async Task<Branch> GetAsync(int id)
        {
            return Data.FirstOrDefaultAsync(a => a.Id.Equals(id)).Result;
        }

        /// <inheritdoc />
        public override IQueryable<Branch> Get() => this.GetAsync().Result;
    }
}
