using Mapster;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    /// <inheritdoc />
    public class CompanyManager : ICompanyManager
    {

        private readonly ILogger<CompanyManager> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyRepository"></param>
        public CompanyManager(ILogger<CompanyManager> logger, ICompanyRepository companyRepository)
        {
            _logger = logger;
            _companyRepository = companyRepository;

        }

        /// <summary>
        /// Getting all branches or first 15 by defolt
        /// </summary>
        public async Task<PagingResult<CompanyModel>> GetCompanies(CompanyFilter filter, Sortable sortable, Paginable paginable)
        {
            IQueryable<Company> queryable = await _companyRepository.GetAsync();

            if (!string.IsNullOrEmpty(filter.State))
            {
                queryable = queryable.Where(x => x.State.ToLower().Equals(filter.State.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.City))
            {
                queryable = queryable.Where(x => x.City.ToLower().Contains(filter.City.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                queryable = queryable.Where(x => x.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.CompanyName))
            {
                queryable = queryable.Where(x => x.CompanyName.ToLower().Contains(filter.CompanyName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.CompanyId))
            {
                queryable = queryable.Where(x => x.Id.Equals(filter.CompanyId));
            }

            // Calculate total count
            var queryableCount = queryable;
            int totalCount = queryableCount.Count();

            // Sorting
            if (sortable.SortBy.ToLower() == "createddate")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    queryable = queryable.OrderByDescending(x => x.CreatedDate);
                }
                else queryable = queryable.OrderBy(x => x.CreatedDate);
            }

            if (sortable.SortBy.ToLower() == "status")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    queryable = queryable.OrderBy(x => x.Status);
                }
                else queryable = queryable.OrderBy(x => x.CreatedDate);
            }
            if (sortable.SortBy.ToLower() == "companyname")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    queryable = queryable.OrderBy(x => x.CompanyName);
                }
                else queryable = queryable.OrderBy(x => x.CreatedDate);
            }

            // Pagination
            queryable = queryable.Skip(paginable.Skip.Value).Take(paginable.Take.Value);

            return new PagingResult<CompanyModel>
            {
                Total = 7,
                ItemsPerPage = paginable.Take.Value,
                Data = queryable.ToList().Adapt<List<CompanyModel>>()
            };
        }
    }
}
