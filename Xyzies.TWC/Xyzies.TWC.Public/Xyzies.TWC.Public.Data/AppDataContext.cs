using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data
{
    public class AppDataContext: DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {

        }

        #region Entities

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<BranchContact> PrimaryContacts { get; set; }

        public DbSet<BranchContactType> BranchContactType { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Branch>().HasOne(i=>i.Company).WithMany(p=>p.Branches);
            builder.Entity<BranchContact>().HasOne(i => i.Branch).WithMany(p => p.BranchContacts);
            builder.Entity<BranchContact>().HasOne(i => i.BranchContactType).WithMany(p => p.BranchContacts);

        }
    }
}
