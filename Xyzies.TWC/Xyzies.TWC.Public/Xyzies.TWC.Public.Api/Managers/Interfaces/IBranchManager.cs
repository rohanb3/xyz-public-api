using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Data.Entities;

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
        Task<PagingResult<BranchModel>> GetBranches(Filter filter, Sortable sortable, Paginable paginable);

        /// <summary>
        /// Getting branches by company all or first 15 by defolt
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filterModel"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<BranchModel>> GetBranchesByCompany(int companyId, Filter filterModel, Sortable sortable, Paginable paginable);
    }
}
