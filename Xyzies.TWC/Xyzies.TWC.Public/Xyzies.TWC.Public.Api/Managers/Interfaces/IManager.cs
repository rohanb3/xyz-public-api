using System.Linq;
using Xyzies.TWC.Public.Api.Controllers;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// Forcing to implement Pagination, Filtering and Sorting
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Filtering settings
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<Branch> Filtering(BranchFilter filter, IQueryable<Branch> query);

        /// <summary>
        /// Sorting settings
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<Branch> Sorting(Sortable filter, IQueryable<Branch> query);

        /// <summary>
        /// Pagination settings
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<Branch> Pagination(Paginable filter, IQueryable<Branch> query);
    }
}
