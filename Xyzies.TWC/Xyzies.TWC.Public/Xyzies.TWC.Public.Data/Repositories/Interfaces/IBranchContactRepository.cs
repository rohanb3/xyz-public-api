using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface IBranchContactRepository
    {
        Task<BranchContact> GetAsync(int id);
    }
}
