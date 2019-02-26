using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Controllers;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// Facade of manage a branch requests
    /// </summary>
    public interface IBranchManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<BranchModel>> GetBranches(BranchFilter filter, Sortable sortable, Paginable paginable);
    }
}
