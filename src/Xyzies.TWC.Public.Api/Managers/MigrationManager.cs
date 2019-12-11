using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Managers
{
    public class MigrationManager : IMigrationManager
    {

        private readonly ITenantRepository _tenantRepository;

        private readonly ICompanyRepository _companyRepository;

        private readonly ICompanyTenantRepository _companyTenantRepository;

        public MigrationManager(ITenantRepository tenantRepository, ICompanyRepository companyRepository, ICompanyTenantRepository companyTenantRepository)
        {
            _companyTenantRepository = companyTenantRepository;
            _companyRepository = companyRepository;
            _tenantRepository = tenantRepository;

        }
        public async Task AssignToTenant(Guid tenantId)
        {
            var isExist = (await _tenantRepository.GetAsync(tenantId)) != null;
            if (!isExist)
            {
                throw new KeyNotFoundException("Tenant not found");
            }
            var existingCompanies = (await _companyTenantRepository.GetAsync(x => x.TenantId == tenantId))
                .Select(x => x.CompanyId)
                .ToHashSet();
            var newCompanyIds = (await _companyRepository.GetAsync(x => !existingCompanies.Contains(x.Id))).Select(x => x.Id).ToHashSet();
            if (!newCompanyIds.Any())
            {
                throw new KeyNotFoundException("No new companies found");
            }
            await _companyTenantRepository.AddRangeAsync(
                newCompanyIds.Select(x =>
                new CompanyTenant
                {
                    CompanyId = x,
                    TenantId = tenantId
                }));
        }
    }
}