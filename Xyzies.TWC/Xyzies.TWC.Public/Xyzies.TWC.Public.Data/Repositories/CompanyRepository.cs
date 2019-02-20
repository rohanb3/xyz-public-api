using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class CompanyRepository : EfCoreBaseRepository<Guid, Company>, ICompanyRepository
    {
        public CompanyRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<IQueryable<Company>> GetAsync() =>
            await Task.FromResult(base.Data.AsQueryable());

        /// <inheritdoc />
        public override IQueryable<Company> Get() => this.GetAsync().Result;

        /// <inheritdoc />
        public async Task<Company> GetAsync(int id)
        {
            return await Data.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        /// <inheritdoc />
        public Company Get(int id)
        {
            return this.GetAsync(id).Result;
        }
    }
}
