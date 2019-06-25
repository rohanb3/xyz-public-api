using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<int, Company>, IDisposable
    {
        Task<bool> SetActivationState(int id, bool isEnable);
        Task<Company> GetAnyCompanyAsync(int id);
    }
}