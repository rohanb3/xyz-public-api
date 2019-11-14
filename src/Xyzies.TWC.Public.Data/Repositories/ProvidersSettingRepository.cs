using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class ProvidersSettingRepository : EfCoreBaseRepository<Guid, TenantSetting>, IRepository<Guid, TenantSetting>, ITenantsSettingRepository
    {
        public ProvidersSettingRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<bool> UpdateAsync(TenantSetting entity)
        {
            var entityToUpdate = base.Data.FirstOrDefault(x => x.TenantId == entity.TenantId);
            if (entityToUpdate == null)
            {
                throw new KeyNotFoundException();
            }
            entityToUpdate.Settings = entity.Settings;
            return await base.UpdateAsync(entityToUpdate);
        }

        public Task<string> GetSettingsByProvider(Guid providerId) => Task.FromResult(
                base.Data.FirstOrDefault(x => x.TenantId == providerId)?.Settings);
    }
}
