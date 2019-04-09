using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities.Azure;

namespace Xyzies.TWC.Public.Data.Repositories.Azure
{
    public interface IAzureCompanyAvatarRepository : IRepository<string, CompanyAvatar>
    {
        Task<string> GetAvatarPath(string companyId);
    }
}
