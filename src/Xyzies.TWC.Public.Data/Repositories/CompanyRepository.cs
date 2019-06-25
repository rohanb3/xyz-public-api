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
                .Include(b => b.Branches)
                .Include(b => b.RequestStatus)
                .Where(x => x.RequestStatus.Name.ToLower() == OnBoardedStatusName));

        /// <inheritdoc />
        public override async Task<Company> GetAsync(int id)
        {
            var companies = await Data
                .Include(x => x.Branches)
                .Include(b => b.RequestStatus)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id) && entity.RequestStatus.Name.ToLower() == OnBoardedStatusName);

            return companies;
        }

        public override Task<bool> UpdateAsync(Company entity)
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
            var company = await this.GetAsync(id);
            if (company == null)
            {
                return false;
            }

            company.IsEnabled = isEnabled;

            return await this.UpdateAsync(company);
        }

        /// <inheritdoc />
        public async Task<Company> GetAnyCompanyAsync(int id)
        => await Data.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }
}
