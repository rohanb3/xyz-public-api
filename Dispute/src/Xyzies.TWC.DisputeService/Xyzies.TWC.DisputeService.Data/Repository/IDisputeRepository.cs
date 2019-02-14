using System;
using Xyzies.TWC.DisputeService.Data.Entity;

namespace Xyzies.TWC.DisputeService.Data.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDisputeRepository : IRepository<Guid, Dispute>, IDisposable
    {

    }
}
