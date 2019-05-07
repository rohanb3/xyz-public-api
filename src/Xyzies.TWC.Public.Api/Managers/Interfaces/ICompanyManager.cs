using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers
{
    /// <summary>
    /// Facade of manage a company requests
    /// </summary>
    public interface ICompanyManager : IDisposable
    {
        /// <summary>
        /// Getting all companies or first 15 by defolt
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        Task<PagingResult<CompanyModel>> GetCompanies(CompanyFilter filter = null, Sortable sortable = null, Paginable paginable = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CompanyModelExtended> GetCompanyById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listUserIds"></param>
        /// <returns></returns>
        Task<PagingResult<CompanyModel>> GetCompanyByUser(List<int> listUserIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        Task<PagingResult<CompanyMin>> GetCompanyNameById(List<int> companyIds);
    }
}
