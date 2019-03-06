﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface IBranchRepository : IRepository<int, Branch>, IDisposable
    {
        Task<bool> SetActivationState(int id, bool isEnabled);
    }
}
