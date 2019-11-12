using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    public class ProviderSettingManager : IProviderSettingManager
    {
        private readonly IProvidersSettingRepository _providersSettingRepository;

        public ProviderSettingManager(IProvidersSettingRepository providersSettingRepository)
        {
            _providersSettingRepository = providersSettingRepository;

        }

        public async Task<ProviderSettingModel> GetProviderSettings(Guid providerId)
        {

            var providerSettingString = await _providersSettingRepository.GetSettingsByProvider(providerId);
            if (string.IsNullOrWhiteSpace(providerSettingString))
            {
                throw new KeyNotFoundException();
            }
            return JsonConvert.DeserializeObject<ProviderSettingModel>(providerSettingString);
        }

        public async Task InsertProviderSettings(Guid providerId, ProviderSettingModel settings)
        {
            await _providersSettingRepository.AddAsync(GetProviderSettingModel(providerId, settings));
        }

        public async Task UpdateProviderSettings(Guid providerId, ProviderSettingModel settings)
        {
            try
            {
                await _providersSettingRepository.UpdateAsync(GetProviderSettingModel(providerId, settings));
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private ProviderSetting GetProviderSettingModel(Guid providerId, ProviderSettingModel settings)
        {
            return new ProviderSetting
            {
                Settings = JsonConvert.SerializeObject(settings),
                ServiceProviderId = providerId
            };
        }
    }
}