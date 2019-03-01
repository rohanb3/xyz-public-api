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
    public class BranchManager : IBranchManager
    {

        private readonly ILogger<BranchManager> _logger = null;
        private readonly IBranchRepository _branchRepository = null;
        private readonly IUserRepository _userRepository = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="branchRepository"></param>
        /// <param name="userRepository"></param>
        public BranchManager(ILogger<BranchManager> logger, IBranchRepository branchRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _branchRepository = branchRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<PagingResult<BranchModel>> GetBranches(Filter filter, Sortable sortable, Paginable paginable)
        {
            IQueryable<Branch> query = await _branchRepository.GetAsync();

            query = Filtering(filter, query);

            // Calculate total count
            var queryableCount = query;
            int totalCount = queryableCount.Count();

            query = Sorting(sortable, query);

            query = Pagination(paginable, query);

            var branches = query.ToList();
            var branchModelList = new List<BranchModel>();

            foreach (var branch in branches)
            {
                var branchModel = branch.Adapt<BranchModel>();
                branchModel.CountSalesRep = _userRepository.GetAsync(x => x.CompanyID.Value == branch.CompanyId).Result.ToList().Count;

                branchModelList.Add(branchModel);
            }

            return new PagingResult<BranchModel>
            {
                Total = totalCount,
                ItemsPerPage = paginable.Take.Value,
                Data = branchModelList
            };
        }

        /// <inheritdoc />
        public async Task<PagingResult<BranchModel>> GetBranchesByCompany(int companyId, Filter filter, Sortable sortable, Paginable paginable)
        {
            IQueryable<Branch> query = await _branchRepository.GetAsync(x => x.CompanyId.Equals(companyId));

            query = Filtering(filter, query);

            var queryableCount = query;
            int totalCount = queryableCount.Count();

            query = Sorting(sortable, query);

            query = Pagination(paginable, query);

            var branches = query.ToList();
            var branchModelList = new List<BranchModel>();

            foreach (var branch in branches)
            {
                var branchModel = branch.Adapt<BranchModel>();
                branchModel.CountSalesRep = _userRepository.GetAsync(x => x.CompanyID.Value == branch.CompanyId).Result.ToList().Count;

                branchModelList.Add(branchModel);
            }

            return new PagingResult<BranchModel>
            {
                Total = totalCount,
                ItemsPerPage = paginable.Take.Value,
                Data = branchModelList
            };
        }

        /// <inheritdoc />
        public IQueryable<Branch> Filtering(Filter filter, IQueryable<Branch> query)
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
                query = query.Where(x => x.BranchName.ToLower().Contains(filter.Name.ToLower()));
            }

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
        public IQueryable<Branch> Sorting(Sortable sortable, IQueryable<Branch> query)
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
                else query = query.OrderBy(x => x.CreatedDate);
            }

            if (sortable.SortBy.ToLower() == "state")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.BranchName);
                }
                else query = query.OrderBy(x => x.CreatedDate);
            }

            if (sortable.SortBy.ToLower() == "city")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.BranchName);
                }
                else query = query.OrderBy(x => x.CreatedDate);
            }

            if (sortable.SortBy.ToLower() == "branchname")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.BranchName);
                }
                else query = query.OrderBy(x => x.CreatedDate);
            }

            if (sortable.SortBy.ToLower() == "branchid")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.BranchName);
                }
                else query = query.OrderBy(x => x.CreatedDate);
            }

            return query;
        }

        /// <inheritdoc />
        public IQueryable<Branch> Pagination(Paginable paginable, IQueryable<Branch> query)
        {
            return query.Skip(paginable.Skip.Value).Take(paginable.Take.Value);
        }

    }
}
