using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.DisputeService.Data.Entity.Order;

namespace Xyzies.TWC.DisputeService.Data.Repository.DisputeOrder
{
    public class OrderRepository : EfCoreBaseRepository<Guid, Order>, IOrderRepository
    {
        public OrderRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
