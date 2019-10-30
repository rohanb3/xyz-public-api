using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Logging;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    public class ServiceProviderManager : IServiceProviderManager
    {
        private readonly ILogger<ServiceProviderManager> _logger = null;
        private readonly IServiceProviderRepository _serviceProviderRepository = null;

        public ServiceProviderManager(ILogger<ServiceProviderManager> logger,
            IServiceProviderRepository serviceProviderRepository)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _serviceProviderRepository = serviceProviderRepository ??
                throw new ArgumentNullException(nameof(_serviceProviderRepository));
        }

        public async Task<Guid> Create(ServiceProviderRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Create] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var serviceProviders = await _serviceProviderRepository.GetAsync();
            if (serviceProviders.Any(x => x.SeviceProviderName.ToLower() == request.Name.ToLower()))
            {
                _logger.LogError($"[Create] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var serviceProvider = request.Adapt<ServiceProvider>();
            serviceProvider.CreatedOn = DateTime.UtcNow;
            return await _serviceProviderRepository.AddAsync(serviceProvider);
        }

        public async Task<IEnumerable<ServiceProviderModel>> Get()
        {
            var serviceProviders = await _serviceProviderRepository.GetAsync();
            return serviceProviders.Adapt<IEnumerable<ServiceProviderModel>>().ToList();
        }

        public async Task Update(Guid id, ServiceProviderRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Update] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var serviceProviders = await _serviceProviderRepository.GetAsync();
            if (serviceProviders.Any(x => x.SeviceProviderName.ToLower() == request.Name.ToLower() && x.Id != id))
            {
                _logger.LogError($"[Update] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var serviceProvider = serviceProviders.FirstOrDefault(x => x.Id == id);

            if (serviceProvider == null)
            {
                _logger.LogError($"[Update] ServiceProvider with parameter: {nameof(id)} not found");
                throw new KeyNotFoundException(nameof(id));
            }

            serviceProvider.SeviceProviderName = request.Name;
            serviceProvider.Phone = request.Phone;
            await _serviceProviderRepository.UpdateAsync(serviceProvider);
        }

    }
}