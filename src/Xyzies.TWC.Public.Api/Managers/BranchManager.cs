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
        public async Task<PagingResult<BranchModel>> GetBranches(BranchFilter filter, Sortable sortable, Paginable paginable)
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
                branchModel.CountSalesRep = _userRepository.GetAsync(x => x.BranchId == branch.Id).Result.ToList().Where(x => x.Role == null ? false : x.Role.Equals("2")).ToList().Count;

                bool userCountfiltr = true;

                if (filter.UserCountFilter.HasValue && branchModel.CountSalesRep != filter.UserCountFilter)
                {
                    userCountfiltr = false;
                }

                if (userCountfiltr)
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
        public async Task<BranchModel> GetBranchById(int Id)
        {
            var branchDetails = await _branchRepository.GetAsync(Id);
            var branchDetailModel = branchDetails.Adapt<BranchModel>();
            branchDetailModel.CountSalesRep = _userRepository.GetAsync(x => x.Company.Id == branchDetailModel.CompanyId).Result.ToList().Count;
            return branchDetailModel;
        }

        /// <inheritdoc />
        public async Task<PagingResult<BranchModel>> GetBranchesByCompany(int companyId, BranchFilter filter, Sortable sortable, Paginable paginable)
        {
            IQueryable<Branch> query = await _branchRepository.GetAsync(x => x.Company.Id == companyId);

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
                branchModel.CountSalesRep = _userRepository.GetAsync(x => x.Company.Id == branch.CompanyId && x.Role.Equals("2")).Result.ToList().Count;

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
        public async Task<List<BranchModel>> GetBranchesByUser(List<int> listUserIds)
        {
            var users = await _userRepository.GetAsync(x => listUserIds.Contains(x.Id));

            var userGroups = users.ToList().GroupBy(x => x.BranchId);

            List<BranchModel> branches = new List<BranchModel>();
            foreach (var group in userGroups)
            {
                BranchModel branchModel = null;
                foreach (var user in group)
                {
                    var branch = await _branchRepository.GetByAsync(x => x.Id == user.BranchId);
                    if (branch == null)
                    {
                        continue;
                    }
                    branchModel = branch.Adapt<BranchModel>();
                    branchModel?.UserIds.Add(user.Id);
                }
                if (branchModel != null)
                {
                    branches.Add(branchModel);
                }
            }
            return branches;
        }

        /// <inheritdoc />
        public async Task<Dictionary<int, string>> GetBranchesById(List<int> branchIds)
        {
            var branches = await _branchRepository.GetAsync(x => branchIds.Contains(x.Id));

            var res = branches.Select(x => KeyValuePair.Create(x.Id, x.BranchName)).ToDictionary(x=>x.Key, x=>x.Value);
            //KeyValuePair ttt = new KeyValuePair("", 12);
            //var userGroups = users.ToList().GroupBy(x => x.BranchId);

            //List<BranchModel> branches = new List<BranchModel>();
            //foreach (var group in userGroups)
            //{
            //    BranchModel branchModel = null;
            //    foreach (var user in group)
            //    {
            //        var branch = await _branchRepository.GetByAsync(x => x.Id == user.BranchId);
            //        if (branch == null)
            //        {
            //            continue;
            //        }
            //        branchModel = branch.Adapt<BranchModel>();
            //        branchModel?.UserIds.Add(user.Id);
            //    }
            //    if (branchModel != null)
            //    {
            //        branches.Add(branchModel);
            //    }
            //}
            return res;
        }

        /// <inheritdoc />
        public IQueryable<Branch> Filtering(BranchFilter branchFilter, IQueryable<Branch> query)
        {
            if (branchFilter.UserIds.Count <= 0)
            {

                if (!string.IsNullOrEmpty(branchFilter.StateFilter))
                {
                    query = query.Where(x => x.State.ToLower().Equals(branchFilter.StateFilter.ToLower()));
                }

                if (!string.IsNullOrEmpty(branchFilter.CityFilter))
                {
                    query = query.Where(x => x.City.ToLower().Contains(branchFilter.CityFilter.ToLower()));
                }

                if (!string.IsNullOrEmpty(branchFilter.EmailFilter))
                {
                    query = query.Where(x => x.Email.ToLower().Contains(branchFilter.EmailFilter.ToLower()));
                }

                if (!string.IsNullOrEmpty(branchFilter.BranchNameFilter))
                {
                    query = query.Where(x => x.BranchName.ToLower().Contains(branchFilter.BranchNameFilter.ToLower()));
                }

                if (branchFilter.IsEnabledFilter.HasValue)
                {
                    query = query.Where(x => x.IsEnabled.Equals(branchFilter.IsEnabledFilter));
                }

                if (branchFilter.BranchIdFilter.HasValue)
                {
                    query = query.Where(x => x.Id == branchFilter.BranchIdFilter);
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _userRepository.Dispose();
            _branchRepository.Dispose();
        }
        
    }
}
