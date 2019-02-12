using IdentityServiceClient.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServiceClient.Service
{
    public interface IClientService
    {
        Task<ResponseModel<List<Profile>>> GetAllUsersAsync();
        Task<bool> CheckHash(string hash);
    }
}
