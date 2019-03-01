using System.Linq;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// Forcing to implement Pagination, Filtering and Sorting
    /// </summary>
    public interface IManager<T>
    {
        /// <summary>
        /// Filtering settings
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> Filtering(Filter filter, IQueryable<T> query);

        /// <summary>
        /// Sorting settings
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> Sorting(Sortable filter, IQueryable<T> query);

        /// <summary>
        /// Pagination settings
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> Pagination(Paginable filter, IQueryable<T> query);
    }
}
