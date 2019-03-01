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
using Xyzies.TWC.Public.Api.Controllers;

namespace Xyzies.TWC.Public.Api.Managers
{
    /// <inheritdoc />
    public class CompanyManager : ICompanyManager
    {

        private readonly ILogger<CompanyManager> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IUserRepository _userRepository = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyRepository"></param>
        /// <param name="userRepository"></param>
        public CompanyManager(ILogger<CompanyManager> logger, ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<PagingResult<CompanyModel>> GetCompanies(Filter filter, Sortable sortable, Paginable paginable)
        {
            IQueryable<Company> query = await _companyRepository.GetAsync();

            query = Filtering(filter, query);

            var queryableCount = query;
            int totalCount = queryableCount.Count();

            query = Sorting(sortable, query);

            query = Pagination(paginable, query);

            var companies = query.ToList();
            var companyModelList = new List<CompanyModel>();

            foreach (var company in companies)
            {
                var companyModel = company.Adapt<CompanyModel>();
                companyModel.CountSalesRep = _userRepository.GetAsync(x => x.CompanyID.Value == company.Id).Result.ToList().Count;
                companyModel.CountBranch = company.Branches.Count;

                companyModelList.Add(companyModel);
            }

            return new PagingResult<CompanyModel>
            {
                Total = 7,
                ItemsPerPage = paginable.Take.Value,
                Data = companyModelList
            };
        }

        /// <inheritdoc />
        public IQueryable<Company> Filtering(Filter filter, IQueryable<Company> query)
        {
            if (!string.IsNullOrEmpty(filter.State))
            {
                query = query.Where(x => x.State.ToLower().Equals(filter.State.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.City))
            {
                query = query.Where(x => x.City.ToLower().Contains(filter.City.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => x.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.CompanyName.ToLower().Contains(filter.Name.ToLower()));
            }

            query = query.Where(x => x.IsEnabled.Equals(filter.IsDisable));

            if (!string.IsNullOrEmpty(filter.Id))
            {
                int id = 0;
                if (int.TryParse(filter.Id, out id))
                {
                    query = query.Where(x => x.Id.Equals(id));
                }
            }
            return query;
        }

        /// <inheritdoc />
        public IQueryable<Company> Sorting(Sortable sortable, IQueryable<Company> query)
        {
            if (sortable.SortBy.ToLower() == "createddate")
            {
                if (sortable.SortOrder.ToLower().Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.CreatedDate);
                }
                else query = query.OrderBy(x => x.CreatedDate);
            }

            if (sortable.SortBy.ToLower() == "status")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.Status);
                }
                else query = query.OrderBy(x => x.Status);
            }

            if (sortable.SortBy.ToLower() == "state")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.State);
                }
                else query = query.OrderBy(x => x.State);
            }

            if (sortable.SortBy.ToLower() == "city")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.City);
                }
                else query = query.OrderBy(x => x.City);
            }

            if (sortable.SortBy.ToLower() == "companyname")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.CompanyName);
                }
                else query = query.OrderBy(x => x.CompanyName);
            }

            if (sortable.SortBy.ToLower() == "companyid")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                else query = query.OrderBy(x => x.Id);
            }

            return query;
        }

        /// <inheritdoc />
        public IQueryable<Company> Pagination(Paginable paginable, IQueryable<Company> query)
        {
            return query.Skip(paginable.Skip.Value).Take(paginable.Take.Value);
        }

    }
}
