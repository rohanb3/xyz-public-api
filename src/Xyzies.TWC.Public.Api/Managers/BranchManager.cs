﻿using Mapster;
using System;
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
    public class BranchManager : IBranchManager
    {
        private readonly ILogger<BranchManager> _logger = null;
        private readonly IBranchRepository _branchRepository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IUserRepository _userRepository = null;
        private readonly string salesRoleId = "2";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="branchRepository"></param>
        /// <param name="companyRepository"></param>
        /// <param name="userRepository"></param>
        public BranchManager(ILogger<BranchManager> logger,
            IBranchRepository branchRepository,
            ICompanyRepository companyRepository,
            IUserRepository userRepository)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _branchRepository = branchRepository ??
                throw new ArgumentNullException(nameof(branchRepository));
            _companyRepository = companyRepository ??
                throw new ArgumentNullException(nameof(companyRepository));
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
        }

        /// <inheritdoc />
        public async Task<PagingResult<BranchModel>> GetBranches(BranchFilter filter = null, Sortable sortable = null, Paginable paginable = null)
        {
            try
            {
                IQueryable<Branch> query = null;

                query = await _branchRepository.GetAsync();
                if (filter != null)
                {
                    query = Filtering(filter, query);
                }
                // Calculate total count
                var queryableCount = query;
                int totalCount = queryableCount.Count();
                if (sortable != null)
                {
                    query = Sorting(sortable, query);
                }
                if (paginable != null)
                {
                    query = Pagination(paginable, query);
                }
                var branches = query.ToList();
                var branchModelList = new List<BranchModel>();

                var allUsersQuery = await _userRepository.GetAsync(x => !string.IsNullOrEmpty(x.Role) ? x.Role.Trim().Equals(salesRoleId) : false);
                var allUsers = allUsersQuery.ToList().GroupBy(x => x.BranchId);

                foreach (var branch in branches)
                {
                    var branchModel = branch.Adapt<BranchModel>();

                    branchModel.CountSalesRep = allUsers.Where(x => x.Key == branch.Id).FirstOrDefault()?.Count();

                    branchModelList.Add(branchModel);
                }

                return new PagingResult<BranchModel>
                {
                    Total = totalCount,
                    ItemsPerPage = paginable?.Take ?? default(int),
                    Data = branchModelList
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <inheritdoc />
        public async Task<BranchModel> GetBranchById(Guid Id)
        {
            var branchDetails = await _branchRepository.GetAsync(Id);
            if (branchDetails == null)
            {
                return null;
            }

            var branchDetailModel = branchDetails.Adapt<BranchModel>();

            var branchUsers = await _userRepository.GetAsync(x => x.BranchId == Id);
            var salesBranchUser = branchUsers.ToList().GroupBy(x => x.Role).AsQueryable();

            branchDetailModel.CountSalesRep = salesBranchUser.Where(x => x.Key == salesRoleId).FirstOrDefault()?.Count();

            return branchDetailModel;
        }

        /// <inheritdoc />
        public async Task<PagingResult<BranchModel>> GetBranchesByCompany(int companyId, BranchFilter filter, Sortable sortable, Paginable paginable)
        {
            var company = await _companyRepository.GetAsync(companyId);

            if(company == null)
            {
                throw new KeyNotFoundException();
            }

            IQueryable<Branch> query = await _branchRepository.GetAsync(x => x.Company.Id == companyId);

            query = Filtering(filter, query);

            var queryableCount = query;
            int totalCount = queryableCount.Count();

            query = Sorting(sortable, query);

            query = Pagination(paginable, query);

            var branches = query.ToList();
            var branchModelList = new List<BranchModel>();

            var allUsersQuery = await _userRepository.GetAsync(x => !string.IsNullOrEmpty(x.Role) ? x.Role.Trim().Equals(salesRoleId) : false);

            foreach (var branch in branches)
            {
                var branchModel = branch.Adapt<BranchModel>();

                branchModel.CountSalesRep = allUsersQuery.Where(x => x.BranchId == branch.Id)?.Count();

                branchModelList.Add(branchModel);
            }

            return new PagingResult<BranchModel>
            {
                Total = totalCount,
                ItemsPerPage = paginable.Take ?? 0,
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
        public async Task<PagingResult<BranchMin>> GetBranchesById(List<Guid> branchIds)
        {
            var branchesQuery = await _branchRepository.GetAsync(x => branchIds.Contains(x.Id));
            int totalCount = branchesQuery != null ? branchesQuery.Count() : default(int);

            var branchModelList = branchesQuery.Select(x => new BranchMin
            {
                Id = x.Id,
                BranchName = x.BranchName,
                CreatedDate = x.CreatedDate

            }).ToList();

            return new PagingResult<BranchMin>
            {
                Total = totalCount,
                ItemsPerPage = 0,
                Data = branchModelList
            };
        }

        /// <inheritdoc />
        private IQueryable<Branch> Filtering(BranchFilter branchFilter, IQueryable<Branch> query)
        {
            // TODO: Fix
            if (branchFilter.UserIds.Count == 0)
            {
                if (!string.IsNullOrEmpty(branchFilter.StateFilter))
                {
                    query = query.Where(x => x.State != null &&  x.State.ToLower().Equals(branchFilter.StateFilter.ToLower()));
                }

                if (!string.IsNullOrEmpty(branchFilter.CityFilter))
                {
                    query = query.Where(x => x.City != null && x.City.ToLower().Contains(branchFilter.CityFilter.ToLower()));
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
        public async Task<BranchMin> GetAnyBranchAsync(BranchMinRequestModel requestModel)
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel));
            }
            Branch branch = null;
            if (requestModel.Id.HasValue)
            {
                branch = await _branchRepository.GetAnyBranchAsync(requestModel.Id.Value);
            }
            else
            {
                branch = await _branchRepository.GetByAsync(x => x.BranchName == requestModel.BranchName);
            }
            if(branch == null)
            {
                throw new KeyNotFoundException();
            }
            return branch.Adapt<BranchMin>();
        }
        /// <inheritdoc />
        public async Task<bool> Update(Guid id, CreateBranchModel request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var branch = await _branchRepository.GetAsync(id);
            if (branch == null)
            {
                throw new KeyNotFoundException();
            }
            branch.BranchName = request.BranchName;
            branch.Email = request.Email;
            branch.Phone = request.Phone;
            branch.Fax = request.Fax;
            branch.State = request.State;
            branch.City = request.City;
            branch.ZipCode = request.ZipCode;
            branch.AddressLine1 = request.AddressLine1;
            branch.AddressLine2 = request.AddressLine2;
            branch.GeoLat = request.GeoLat;
            branch.GeoLng = request.GeoLng;
            branch.IsEnabled = request.IsEnabled;
            branch.CompanyId = request.CompanyId;
            branch.BranchContacts = new List<BranchContact>() { request.BranchContacts };

            return await _branchRepository.UpdateAsync(branch);
        }

        /// <inheritdoc />
        private IQueryable<Branch> Sorting(Sortable sortable, IQueryable<Branch> query)
        {
            if (sortable.SortBy.ToLower() == "createddate")
            {
                if (sortable.SortOrder.ToLower().Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.CreatedDate);
                }
                else query = query.OrderBy(x => x.CreatedDate);
            }

            // NOTE: Sorting by active status
            if (sortable.SortBy.ToLower() == "status")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.IsEnabled);
                }
                else query = query.OrderBy(x => x.IsEnabled);
            }

            if (sortable.SortBy.ToLower() == "state")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.State);
                }
                else query = query.OrderBy(x => x.State);
            }

            // NOTE: The same as above
            if (sortable.SortBy.ToLower() == "isenabled")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.IsEnabled);
                }
                else query = query.OrderBy(x => x.IsEnabled);
            }

            if (sortable.SortBy.ToLower() == "city")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.City);
                }
                else query = query.OrderBy(x => x.City);
            }

            if (sortable.SortBy.ToLower() == "branchname")
            {
                if (sortable.SortOrder.Equals("desc"))
                {
                    query = query.OrderByDescending(x => x.BranchName);
                }
                else query = query.OrderBy(x => x.BranchName);
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
        private IQueryable<Branch> Pagination(Paginable paginable, IQueryable<Branch> query)
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
            _branchRepository.Dispose();
        }
#pragma warning restore CA1063 // Implement IDisposable Correctly
    }
}
