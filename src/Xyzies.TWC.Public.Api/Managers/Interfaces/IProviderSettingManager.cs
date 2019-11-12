using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface IProviderSettingManager
    {
        Task<ProviderSettingModel> GetProviderSettings(Guid providerId);
        Task InsertProviderSettings(Guid providerId, ProviderSettingModel settings);
        Task UpdateProviderSettings(Guid providerId, ProviderSettingModel settings);
    }
}