using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface ITenantSettingManager
    {
        Task<TenantSettingModel> GetTenantSettings(Guid providerId);
        Task InsertTenantSettings(Guid providerId, TenantSettingModel settings);
        Task UpdateTenantSettings(Guid providerId, TenantSettingModel settings);
    }
}