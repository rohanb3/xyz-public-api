using System;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface IBranchRepository : IRepository<int, Branch>, IDisposable
    {
        EntityState BranchActivator(int id, bool is_enabled);
    }
}
