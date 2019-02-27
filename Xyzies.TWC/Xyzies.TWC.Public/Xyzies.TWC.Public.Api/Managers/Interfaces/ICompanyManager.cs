using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICompanyManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<CompanyModel>> GetCompanies(CompanyFilter filter, Sortable sortable, Paginable paginable);
    }
}
