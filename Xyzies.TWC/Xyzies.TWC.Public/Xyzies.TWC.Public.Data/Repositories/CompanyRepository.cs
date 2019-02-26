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
        public override async Task<Company> GetAsync(int id)
        {
            var branches = await Data
                .Include(x => x.Branches)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

            return branches;
        }
    }
}
