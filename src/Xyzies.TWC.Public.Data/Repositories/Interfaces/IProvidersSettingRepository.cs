using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;

namespace Xyzies.TWC.Public.Data.Repositories.Interfaces
{
    public interface IProvidersSettingRepository : IRepository<Guid, ProviderSetting>
    {
        Task<string> GetSettingsByProvider(Guid providerId);
    }
}