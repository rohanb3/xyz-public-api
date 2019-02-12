using System;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.DisputeService.Data.Entity;

namespace Xyzies.TWC.DisputeService.Data
{
    public class DisputeDataContext : DbContext
    {
        public DisputeDataContext(DbContextOptions<DisputeDataContext> options)
            : base(options)
        {

        }

        #region Entities

        public DbSet<Dispute> Disputes { get; set; }

        #endregion
    }
}
