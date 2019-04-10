using Mapster;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    /// <inheritdoc />
    public class CompanyManager : ICompanyManager
    {

        private readonly ILogger<CompanyManager> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IUserRepository _userRepository = null;
        private readonly string salesRoleId = "2";

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

            var allUsersQuery = await _userRepository.GetAsync(x => !string.IsNullOrEmpty(x.Role) ? x.Role.Trim().Equals(salesRoleId) : false);
            var allUsers = allUsersQuery.ToList().GroupBy(x => x.CompanyId).AsQueryable();

            foreach (var company in companies)
            {
                var companyModel = company.Adapt<CompanyModel>();

                companyModel.CountSalesRep = allUsers.Where(x => x.Key == company.Id)?.Count();
                companyModel.CountBranch = company.Branches.Count;

                companyModelList.Add(companyModel);
            }

            return new PagingResult<CompanyModel>
            {
                Total = totalCount,
                ItemsPerPage = paginable.Take ?? 0,
                Data = companyModelList
            };
        }

        /// <inheritdoc />
        public async Task<CompanyModel> GetCompanyById(int Id)
        {
            var companyDetails = await _companyRepository.GetAsync(Id);
            var companyDetailModel = companyDetails.Adapt<CompanyModel>();

            if (companyDetailModel != null)
            {
                var usersByCompany = await _userRepository.GetAsync(x => x.CompanyId == Id);
                var userByRoleCompany = usersByCompany.ToList().GroupBy(x => x.Role).AsQueryable();

                companyDetailModel.CountSalesRep = userByRoleCompany.Where(x => x.Key == salesRoleId).FirstOrDefault()?.Count() ?? 0;
                companyDetailModel.CountBranch = companyDetails.Branches.Count;
            }

            return companyDetailModel;

        }

        /// <inheritdoc />
        public async Task<PagingResult<CompanyModel>> GetCompanyByUser(List<int> listUserIds)
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
            return new PagingResult<CompanyModel>
            {
                Total = 0,
                ItemsPerPage = 0,
                Data = companies
            };
        }

        /// <inheritdoc />
        public async Task<PagingResult<CompanyMin>> GetCompanyNameById(List<int> companyIds)
        {
            var companiesQuery = await _companyRepository.GetAsync(x => companyIds.Contains(x.Id));
            var totalCount = companiesQuery != null ? companiesQuery.Count() : default(int);

            var companies = companiesQuery.Select(x => new CompanyMin
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                CreatedDate = x.CreatedDate
            });

            var listCompany = companies.ToList();

            return new PagingResult<CompanyMin>
            {
                Total = totalCount,
                ItemsPerPage = 0,
                Data = listCompany
            };
        }

        /// <inheritdoc />
        private IQueryable<Company> Filtering(CompanyFilter companyFilter, IQueryable<Company> query)
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

            if (companyFilter.CompanyNameFilter != null && companyFilter.CompanyNameFilter.Count > 0)
            {
                query = query.Where(x => companyFilter.CompanyNameFilter.Contains(x.CompanyName));
            }

            if (companyFilter.DateFrom.HasValue)
            {
                query = query.Where(x => companyFilter.DateFrom < x.CreatedDate);
            }

            if (companyFilter.DateTo.HasValue)
            {
                query = query.Where(x => companyFilter.DateTo > x.CreatedDate);
            }

            if (companyFilter.IsEnabledFilter.HasValue)
            {
                query = query.Where(x => x.IsEnabled.Equals(companyFilter.IsEnabledFilter));
            }

            if (companyFilter.CompanyIdFilter.HasValue)
            {
                query = query.Where(x => x.Id == companyFilter.CompanyIdFilter);
            }

            if (!string.IsNullOrEmpty(companyFilter.SearchFilter))
            {
                query = query.Where(x => x.CompanyName.ToLower().Contains(companyFilter.SearchFilter.ToLower()));
            }

            return query;
        }

        /// <inheritdoc />
        private IQueryable<Company> Sorting(Sortable sortable, IQueryable<Company> query)
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

            if (sortable.SortBy.ToLower() == "id")
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
        private IQueryable<Company> Pagination(Paginable paginable, IQueryable<Company> query)
        {
            if (paginable.Take.HasValue && paginable.Skip.HasValue)
            {
                return query.Skip(paginable.Skip.Value).Take(paginable.Take.Value);
            }
            return query;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _userRepository.Dispose();
            _companyRepository.Dispose();
        }
#pragma warning restore CA1063 // Implement IDisposable Correctly
    }
}
