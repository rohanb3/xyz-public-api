using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities.Azure;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICompanyAvatarsManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyAvatar"></param>
        /// <returns></returns>
        Task<bool> UploadCompanyAvatarAsync(CompanyAvatar companyAvatar);
    }
}
