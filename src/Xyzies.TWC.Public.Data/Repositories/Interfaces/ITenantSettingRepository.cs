using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface ITenantSettingRepository : IRepository<Guid, TenantSetting>
    {
        Task<string> GetSettingsByProvider(Guid providerId);
    }
}