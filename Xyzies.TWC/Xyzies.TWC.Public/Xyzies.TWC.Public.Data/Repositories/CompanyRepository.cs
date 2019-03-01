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

        /// <inheritdoc />
        public override async Task<IQueryable<Company>> GetAsync() =>
            await Task.FromResult(base.Data
                .Include(b => b.Branches));

        /// <inheritdoc />
        public override async Task<Company> GetAsync(int id)
        {
            var companies = await Data
                .Include(x => x.Branches)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

            return companies;
        }
    }
}
