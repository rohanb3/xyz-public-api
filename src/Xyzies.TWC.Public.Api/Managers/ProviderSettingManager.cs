using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    public class TenantSettingManager : ITenantSettingManager
    {
        private readonly ITenantSettingRepository _tenantsSettingRepository;

        public TenantSettingManager(ITenantSettingRepository providersSettingRepository)
        {
            _tenantsSettingRepository = providersSettingRepository;
        }

        public async Task<TenantSettingModel> GetTenantSettings(Guid providerId)
        {

            var providerSettingString = await _tenantsSettingRepository.GetSettingsByProvider(providerId);
            if (string.IsNullOrWhiteSpace(providerSettingString))
            {
                throw new KeyNotFoundException();
            }
            return JsonConvert.DeserializeObject<TenantSettingModel>(providerSettingString);
        }

        public async Task<TenantSettingModel> GetTenantSettingsByCompanyId(int companyId)
        {
            var setting = await _tenantsSettingRepository.GetByAsync(x => x.Tenant.Companies.Select(c => c.CompanyId).Contains(companyId));
            if (setting == null)
            {
                throw new KeyNotFoundException();
            }
            return JsonConvert.DeserializeObject<TenantSettingModel>(setting.Settings);
        }

        public async Task InsertTenantSettings(Guid providerId, TenantSettingModel settings)
        {
            await _tenantsSettingRepository.AddAsync(GetTenantSettingModel(providerId, settings));
        }

        public async Task UpdateTenantSettings(Guid providerId, TenantSettingModel settings)
        {
            try
            {
                await _tenantsSettingRepository.UpdateAsync(GetTenantSettingModel(providerId, settings));
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private TenantSetting GetTenantSettingModel(Guid providerId, TenantSettingModel settings)
        {
            return new TenantSetting
            {
                Settings = JsonConvert.SerializeObject(settings),
                TenantId = providerId
            };
        }
    }
}