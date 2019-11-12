using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class ProvidersSettingRepository : EfCoreBaseRepository<Guid, ProviderSetting>, IRepository<Guid, ProviderSetting>, IProvidersSettingRepository
    {
        public ProvidersSettingRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }

        public override async Task<bool> UpdateAsync(ProviderSetting entity)
        {
            var entityToUpdate = base.Data.FirstOrDefault(x => x.ServiceProviderId == entity.ServiceProviderId);
            if (entityToUpdate == null)
            {
                throw new KeyNotFoundException();
            }
            entityToUpdate.Settings = entity.Settings;
            return await base.UpdateAsync(entityToUpdate);
        }

        public Task<string> GetSettingsByProvider(Guid providerId) => Task.FromResult(
                base.Data.FirstOrDefault(x => x.ServiceProviderId == providerId)?.Settings);
    }
}
