using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Logging;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Models.Filters;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    public class ServiceProviderManager : IServiceProviderManager
    {
        private readonly ILogger<ServiceProviderManager> _logger = null;
        private readonly IServiceProviderRepository _serviceProviderRepository = null;
        private readonly ICompanyManager _companyManager = null;

        public ServiceProviderManager(ILogger<ServiceProviderManager> logger,
            IServiceProviderRepository serviceProviderRepository,
            ICompanyManager companyManager)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _serviceProviderRepository = serviceProviderRepository ??
                throw new ArgumentNullException(nameof(_serviceProviderRepository));
            _companyManager = companyManager ??
                throw new ArgumentNullException(nameof(_companyManager));
        }

        public async Task<Guid> Create(ServiceProviderRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Create] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var serviceProviders = await _serviceProviderRepository.GetAsync();
            if (serviceProviders.Any(x => x.ServiceProviderName.ToLower() == request.Name.ToLower()))
            {
                _logger.LogError($"[Create] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var serviceProvider = request.Adapt<ServiceProvider>();
            serviceProvider.CreatedOn = DateTime.UtcNow;
            return await _serviceProviderRepository.AddAsync(serviceProvider);
        }

        public async Task<IEnumerable<ServiceProviderModel>> Get(TenantFilterModel filterModel)
        {
            var serviceProvidersList = (await _serviceProviderRepository.GetAsync(x => filterModel.TenantIds.Contains(x.Id))).ToList();
            var companyIds = serviceProvidersList.SelectMany(x => x.Companies.Select(c => c.CompanyId)).Distinct().ToList();
            var companies = (await _companyManager.GetCompanies(new CompanyFilter { CompanyIds = companyIds })).Data.ToHashSet();
            var result = new List<ServiceProviderModel>();
            foreach (var serviceProvider in serviceProvidersList)
            {
                var model = new ServiceProviderModel
                {
                    Id = serviceProvider.Id,
                    Name = serviceProvider.ServiceProviderName,
                    Phone = serviceProvider.Phone,
                    Companies = companies.Where(x => serviceProvider.Companies.Select(c => c.CompanyId).Contains(x.Id)).ToList()
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<ServiceProviderModel> GetExtended(Guid id)
        {
            var serviceProvider = await _serviceProviderRepository.GetByAsync(x => x.Id == id);
            if (serviceProvider == null)
            {
                throw new KeyNotFoundException();
            }
            return serviceProvider.Adapt<ServiceProviderModel>();
        }


        public async Task<ServiceProviderSingleModel> GetSingle(Guid id)
        {
            var serviceProvider = await _serviceProviderRepository.GetByAsync(x => x.Id == id);
            if (serviceProvider == null)
            {
                throw new KeyNotFoundException();
            }
            return serviceProvider.Adapt<ServiceProviderSingleModel>();
        }

        public async Task<ServiceProviderModel> Get(int companyId)
        {
            var serviceProvider = await _serviceProviderRepository.GetServiceProviderByCompany(companyId);
            if (serviceProvider == null)
            {
                throw new KeyNotFoundException();
            }
            return serviceProvider.Adapt<ServiceProviderModel>();
        }

        public async Task Update(Guid id, ServiceProviderRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Update] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var serviceProviders = await _serviceProviderRepository.GetAsync();
            if (serviceProviders.Any(x => x.ServiceProviderName.ToLower() == request.Name.ToLower() && x.Id != id))
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

            serviceProvider.ServiceProviderName = request.Name;
            serviceProvider.Phone = request.Phone;
            await _serviceProviderRepository.UpdateAsync(serviceProvider);
        }

        public async Task<ServiceProviderSingleModel> GetSingle(int companyId)
        {
            var serviceProvider = await _serviceProviderRepository.GetServiceProviderByCompany(companyId);
            if (serviceProvider == null)
            {
                throw new KeyNotFoundException();
            }
            return serviceProvider.Adapt<ServiceProviderSingleModel>();
        }

        public async Task UpdateByCompanyId(int companyId, ServiceProviderRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Update] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var serviceProviders = await _serviceProviderRepository.GetAsync();
            if (serviceProviders.Any(x => x.ServiceProviderName.ToLower() == request.Name.ToLower() &&
                                          x.Companies.Select(c => c.Id).Contains(companyId)))
            {
                _logger.LogError($"[Update] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var serviceProvider = serviceProviders.FirstOrDefault(x => x.Companies.Select(c => c.Id).Contains(companyId));

            if (serviceProvider == null)
            {
                _logger.LogError($"[Update] ServiceProvider with parameter: {nameof(companyId)} not found");
                throw new KeyNotFoundException(nameof(companyId));
            }

            serviceProvider.ServiceProviderName = request.Name;
            serviceProvider.Phone = request.Phone;
            await _serviceProviderRepository.UpdateAsync(serviceProvider);
        }
    }
}