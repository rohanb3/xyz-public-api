using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Managers.Relation
{
    /// <summary>
    /// Service to send requests to relation microservices
    /// </summary>
    public interface IRelationService
    {
        /// <summary>
        /// Get user by his Cable Portal Id
        /// </summary>
        /// <param name="cpUserId">Cable Portal user id</param>
        /// <returns>User's profile or null if not found</returns>
        Task<User> GetAzureUserByCPUserIdAsync(int cpUserId);

        /// <summary>
        /// Get user from current call in VSP by Cable Portal user's id
        /// </summary>
        /// <param name="cpUserId">Cable Portal user's id</param>
        /// <returns></returns>
        Task<User> GetUserOnCallWithIdAsync(int cpUserId);

        /// <summary>
        /// Get user by his Azure AD B2C Id
        /// </summary>
        /// <param name="objectId">Azure AD B2C user id</param>
        /// <returns>User's profile or null if not found</returns>
        Task<User> GetAzureUserByObjectIdAsync(string objectId);
    }
}
