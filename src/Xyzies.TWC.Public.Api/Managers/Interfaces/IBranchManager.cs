using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Data.Entities;
using System.Collections.Generic;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// Facade of manage a branch requests
    /// </summary>
    public interface IBranchManager : IManager<Branch>
    {
        /// <summary>
        /// Getting all branches or first 15 by defolt
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<BranchModel>> GetBranches(BranchFilter filter, Sortable sortable, Paginable paginable);

        /// <summary>
        /// Getting branches by company all or first 15 by defolt
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filterModel"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<BranchModel>> GetBranchesByCompany(int companyId, BranchFilter filterModel, Sortable sortable, Paginable paginable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BranchModel> GetBranchById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listUsersId"></param>
        /// <returns></returns>
        Task<List<BranchModel>> GetBranchesByUser(List<int> listUsersId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchIds"></param>
        /// <returns></returns>
        Task<Dictionary<int,string>> GetBranchesById(List<int> branchIds);
    }
}
