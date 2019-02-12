using System;
using Xyzies.TWC.DisputeService.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Xyzies.TWC.DisputeService.Data.Repository
{
    public sealed class GenericRepository<TEntity> : EfCoreBaseRepository<Guid, TEntity>
        where TEntity : class, IEntity<Guid>
    {
        public GenericRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
