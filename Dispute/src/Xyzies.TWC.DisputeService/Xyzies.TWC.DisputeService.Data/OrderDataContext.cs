using System;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.DisputeService.Data.Entity;
using Xyzies.TWC.DisputeService.Data.Entity.Order;

namespace Xyzies.TWC.DisputeService.Data
{
    public class OrderDataContext : DbContext
    {
        public OrderDataContext(DbContextOptions<OrderDataContext> options)
            : base(options)
        {

        }

        #region Entities

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderInstall> OrderInstalls { get; set; }

        public virtual DbSet<OfferGroup> OfferGroups { get; set; }

        #endregion
    }
}
