using Microsoft.EntityFrameworkCore;
using System;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<int, Company>, IDisposable
    {
        EntityState CompanyActivator(int id, bool is_enabled);
    }
}