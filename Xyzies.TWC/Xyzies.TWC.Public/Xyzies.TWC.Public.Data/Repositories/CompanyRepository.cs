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
        /// <inheritdoc />
        public override async Task<IQueryable<Company>> GetAsync() =>
            await Task.FromResult(base.Data
                .AsQueryable());

        /// <inheritdoc />
        public override async Task<Company> GetAsync(int id) =>
            await Data.Include(r => r.Branches).FirstOrDefaultAsync<Company>(entity => entity.Id.Equals(id));

        /// <inheritdoc />
        public override int Add(Company entity)
        {
            var id = Data.Add(entity).Entity.Id;
            return id;
        }
    }
}
