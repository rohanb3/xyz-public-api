using IdentityServiceClient.Models;
using IdentityServiceClient.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServiceClient.Service
{
    public interface IIdentityManager
    {

        #region Users
        Task<ResponseModel<List<Profile>>> GetAllUsersAsync();
        Task<ResponseModel<Profile>> GetUserByIdAsync(string id);
        Task<ResponseModel> UpdateUserByIdAsync(string id, BaseProfile model);
        Task<ResponseModel> DeleteUserByIdAsync(string id);
        Task<ResponseModel> CreateUserAsync(ProfileCreatable model);
        #endregion

        #region Permissions
        Task<ResponseModel> CheckPermission(string scope, string policy);
        #endregion

    }
}
