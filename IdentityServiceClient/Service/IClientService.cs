using IdentityServiceClient.Models;
using IdentityServiceClient.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServiceClient.Service
{
    public interface IClientService
    {
        Task<ResponseModel<List<Profile>>> GetAllUsersAsync();
        Task<bool> CheckHash(string hash);
    }
}
