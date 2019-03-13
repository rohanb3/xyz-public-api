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
        public async Task<PagingResult<CompanyModel>> GetCompanies(CompanyFilter filter, Sortable sortable, Paginable paginable)
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
                companyModel.CountSalesRep = _userRepository.GetAsync(x => x.Company.Id == company.Id).Result.ToList().Where(x => x.Role == null ? false : x.Role.Equals("2")).ToList().Count;

                companyModel.CountBranch = company.Branches.Count;

                bool userCountfiltr = true;
                bool branchCountfiltr = true;

                if (filter.UserCountFilter.HasValue && companyModel.CountSalesRep != filter.UserCountFilter)
                {
                    userCountfiltr = false;
                }

                if (filter.BranchCountFilter.HasValue && companyModel.CountBranch != filter.BranchCountFilter)
                {
                    userCountfiltr = false;
                }

                if (userCountfiltr && branchCountfiltr)
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
        public async Task<CompanyModel> GetCompanyById(int Id)
        {
            var companyDetails = await _companyRepository.GetAsync(Id);
            var companyDetailModel = companyDetails.Adapt<CompanyModel>();
            companyDetailModel.CountSalesRep = _userRepository.GetAsync(x => x.Company.Id == companyDetailModel.Id).Result.ToList().Count;
            companyDetailModel.CountBranch = companyDetails.Branches.Count;
            return companyDetailModel;
        }

        /// <inheritdoc />
        public async Task<List<CompanyModel>> GetCompanyByUser(List<int> listUserIds)
        {
            var users = await _userRepository.GetAsync(x => listUserIds.Contains(x.Id));

            var userGroups = users.ToList().GroupBy(x => x.CompanyId);

            List<CompanyModel> companies = new List<CompanyModel>();
            foreach (var grouph in userGroups)
            {
                CompanyModel companyModel = null;
                foreach (var user in grouph)
                {
                    var company = await _companyRepository.GetByAsync(x => x.Id == user.CompanyId);
                    if (company == null)
                    {
                        continue;
                    }
                    companyModel = company.Adapt<CompanyModel>();
                    companyModel.UserIds.Add(user.Id);
                }
                if (companyModel != null)
                {
                    companies.Add(companyModel);
                }
            }
            return companies;
        }

        /// <inheritdoc />
        public async Task<List<CompanyMin>> GetCompanyNameById(List<int> companyIds)
        {
            var companies = await _companyRepository.GetAsync(x => companyIds.Contains(x.Id));

            var res = companies.Select(x => new CompanyMin
            {
                Id = x.Id,
                CompanyName = x.CompanyName
            }).ToList();

            //KeyValuePair.Create(x.Id, x.CompanyName)).ToDictionary(x=>x.Key, x=>x.Value);

            return res;
        }

        /// <inheritdoc />
        public IQueryable<Company> Filtering(CompanyFilter companyFilter, IQueryable<Company> query)
        {
            if (!string.IsNullOrEmpty(companyFilter.StateFilter))
            {
                query = query.Where(x => x.State.ToLower().Equals(companyFilter.StateFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(companyFilter.CityFilter))
            {
                query = query.Where(x => x.City.ToLower().Contains(companyFilter.CityFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(companyFilter.EmailFilter))
            {
                query = query.Where(x => x.Email.ToLower().Contains(companyFilter.EmailFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(companyFilter.CompanyNameFilter))
            {
                query = query.Where(x => x.CompanyName.ToLower().Contains(companyFilter.CompanyNameFilter.ToLower()));
            }

            if (companyFilter.IsEnabledFilter.HasValue)
            {
                query = query.Where(x => x.IsEnabled.Equals(companyFilter.IsEnabledFilter));
            }

            if (companyFilter.CompanyIdFilter.HasValue)
            {
                query = query.Where(x => x.Id == companyFilter.CompanyIdFilter);
            }

            return query;
        }

        /// <inheritdoc />
        public IQueryable<Company> Sorting(Sortable sortable, IQueryable<Company> query)
        {
            if (sortable.SortBy.ToLower() == "createddate" && sortable.SortOrder.ToLower().Equals("asc"))
            {
                query = query.OrderBy(x => x.CreatedDate);
            }
            else query = query.OrderByDescending(x => x.CreatedDate);

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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _userRepository.Dispose();
            _companyRepository.Dispose();
        }
    }
}
