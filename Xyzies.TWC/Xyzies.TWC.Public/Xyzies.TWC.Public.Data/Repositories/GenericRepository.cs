using System;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public sealed class GenericRepository<TEntity> : EfCoreBaseRepository<Guid, TEntity>
        where TEntity : class, IEntity<Guid>
    {
        public GenericRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }
    }
}
