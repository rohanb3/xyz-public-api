using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// Facade of manage a company requests
    /// </summary>
    public interface ICompanyManager : IManager<Company>
    {
        /// <summary>
        /// Getting all companies or first 15 by defolt
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<CompanyModel>> GetCompanies(CompanyFilter filter, Sortable sortable, Paginable paginable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CompanyModel> GetCompanyById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listUsersId"></param>
        /// <returns></returns>
        Task<List<CompanyModel>> GetCompanyByUser(List<int> listUsersId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        Task<List<CompanyMin>> GetCompanyNameById(List<int> companyIds);
    }
}
