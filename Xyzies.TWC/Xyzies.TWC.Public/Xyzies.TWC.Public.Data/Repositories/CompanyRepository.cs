using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class CompanyRepository : EfCoreBaseRepository<int, Company>, ICompanyRepository
    {
        public CompanyRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<IQueryable<Company>> GetAsync() =>
            await Task.FromResult(base.Data
                .AsQueryable());

        public override async Task<Company> GetAsync(int id) =>
            await Data.Include(r => r.Branches).FirstOrDefaultAsync<Company>(entity => entity.Id.Equals(id));
    }
}
