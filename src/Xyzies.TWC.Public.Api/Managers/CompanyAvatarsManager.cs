using System;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Data.Entities.Azure;
using Xyzies.TWC.Public.Data.Repositories.Azure;

namespace Xyzies.TWC.Public.Api.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyAvatarsManager : ICompanyAvatarsManager
    {
        private readonly IAzureCompanyAvatarRepository _azureCompanyAvatarRepository = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="azureCompanyAvatarRepository"></param>
        public CompanyAvatarsManager(IAzureCompanyAvatarRepository azureCompanyAvatarRepository)
        {
            _azureCompanyAvatarRepository = azureCompanyAvatarRepository ?? throw new ArgumentNullException(nameof(azureCompanyAvatarRepository));
        }

        /// <summary>
        /// Asynchronic uploads companyAvatar
        /// </summary>
        /// <param name="companyAvatar"></param>
        /// <returns></returns>
        public async Task<bool> UploadCompanyAvatarAsync(CompanyAvatar companyAvatar)
        {
            var response = await _azureCompanyAvatarRepository.AddAsync(companyAvatar);

            return response == "Uploaded";
        }
    }
}
