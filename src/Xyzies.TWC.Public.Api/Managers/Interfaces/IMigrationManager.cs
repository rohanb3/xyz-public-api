using System;
using System.Threading.Tasks;

namespace Xyzies.TWC.Public.Api.Managers.Interfaces
{
    public interface IMigrationManager
    {
        Task AssignToTenant(Guid tenantId);
    }
}