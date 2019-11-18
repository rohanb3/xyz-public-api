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
using Xyzies.TWC.Public.Data.Entities.TenantEntities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    public class TenantManager : ITenantManager
    {
        private readonly ILogger<TenantManager> _logger = null;
        private readonly ITenantRepository _tenantRepository = null;
        private readonly ICompanyTenantRepository _companyTenantRepository = null;
        private readonly ICompanyManager _companyManager = null;

        public TenantManager(ILogger<TenantManager> logger,
            ITenantRepository tenantRepository,
            ICompanyManager companyManager,
            ICompanyTenantRepository companyTenantRepository)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _tenantRepository = tenantRepository ??
                throw new ArgumentNullException(nameof(_tenantRepository));
            _companyManager = companyManager ??
                throw new ArgumentNullException(nameof(_companyManager));
            _companyTenantRepository = companyTenantRepository ??
                throw new ArgumentNullException(nameof(_companyTenantRepository));
        }

        public async Task<Guid> Create(TenantRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Create] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var tenants = await _tenantRepository.GetAsync();
            if (tenants.Any(x => x.TenantName.ToLower() == request.Name.ToLower()))
            {
                _logger.LogError($"[Create] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var tenant = request.Adapt<Tenant>();
            tenant.CreatedOn = DateTime.UtcNow;
            return await _tenantRepository.AddAsync(tenant);
        }

        public async Task<IEnumerable<TenantModel>> Get(TenantFilterModel filterModel)
        {
            var tenantsList = (await _tenantRepository.GetAsync(x => filterModel.TenantIds.Contains(x.Id))).ToList();
            var companyIds = tenantsList.SelectMany(x => x.Companies.Select(c => c.CompanyId)).Distinct().ToList();
            var companies = (await _companyManager.GetCompanies(new CompanyFilter { CompanyIds = companyIds })).Data.ToHashSet();
            var result = new List<TenantModel>();
            foreach (var tenant in tenantsList)
            {
                var model = new TenantModel
                {
                    Id = tenant.Id,
                    Name = tenant.TenantName,
                    Phone = tenant.Phone,
                    Companies = companies.Where(x => tenant.Companies.Select(c => c.CompanyId).Contains(x.Id)).ToList()
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<TenantModel> GetExtended(Guid id)
        {
            var tenant = await _tenantRepository.GetByAsync(x => x.Id == id);
            if (tenant == null)
            {
                throw new KeyNotFoundException();
            }
            return tenant.Adapt<TenantModel>();
        }


        public async Task<TenantSingleModel> GetSingle(Guid id)
        {
            var tenant = await _tenantRepository.GetByAsync(x => x.Id == id);
            if (tenant == null)
            {
                throw new KeyNotFoundException();
            }
            return tenant.Adapt<TenantSingleModel>();
        }

        public async Task<TenantModel> Get(int companyId)
        {
            var tenant = await _tenantRepository.GetTenantByCompany(companyId);
            if (tenant == null)
            {
                throw new KeyNotFoundException();
            }
            return tenant.Adapt<TenantModel>();
        }

        public async Task Update(Guid id, TenantRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Update] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var tenants = await _tenantRepository.GetAsync();
            if (tenants.Any(x => x.TenantName.ToLower() == request.Name.ToLower() && x.Id != id))
            {
                _logger.LogError($"[Update] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var tenant = tenants.FirstOrDefault(x => x.Id == id);

            if (tenant == null)
            {
                _logger.LogError($"[Update] Tenant with parameter: {nameof(id)} not found");
                throw new KeyNotFoundException(nameof(id));
            }

            tenant.TenantName = request.Name;
            tenant.Phone = request.Phone;
            await _tenantRepository.UpdateAsync(tenant);
        }

        public async Task<TenantSingleModel> GetSingle(int companyId)
        {
            var tenant = await _tenantRepository.GetTenantByCompany(companyId);
            if (tenant == null)
            {
                throw new KeyNotFoundException();
            }
            return tenant.Adapt<TenantSingleModel>();
        }

        public async Task UpdateByCompanyId(int companyId, TenantRequest request)
        {
            if (request == null)
            {
                _logger.LogError($"[Update] Argument {nameof(request)} is null");
                throw new ArgumentNullException(nameof(request));
            }

            var tenants = await _tenantRepository.GetAsync();
            if (tenants.Any(x => x.TenantName.ToLower() == request.Name.ToLower() &&
                                          x.Companies.Select(c => c.Id).Contains(companyId)))
            {
                _logger.LogError($"[Update] Duplicate parameter: {nameof(request.Name)}");
                throw new DuplicateNameException();
            }

            var tenant = tenants.FirstOrDefault(x => x.Companies.Select(c => c.Id).Contains(companyId));

            if (tenant == null)
            {
                _logger.LogError($"[Update] Tenant with parameter: {nameof(companyId)} not found");
                throw new KeyNotFoundException(nameof(companyId));
            }

            tenant.TenantName = request.Name;
            tenant.Phone = request.Phone;
            await _tenantRepository.UpdateAsync(tenant);
        }

        public async Task CreateRelation(int companyId, Guid tenantId)
        {

            var isCompanyExist = (await _companyManager.GetCompanyById(companyId)) != null;
            if (!isCompanyExist)
            {
                throw new KeyNotFoundException("Company is not exists");
            }
            var isTenantExists = (await _tenantRepository.GetAsync(tenantId)) != null;
            if (!isTenantExists)
            {
                throw new KeyNotFoundException("Tenant is not exists");
            }
            await _companyTenantRepository.AddAsync(new CompanyTenant { CompanyId = companyId, TenantId = tenantId });
        }
    }
}