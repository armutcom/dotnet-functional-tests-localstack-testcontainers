using Armut.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armut.Api.Core
{
    public sealed class ArmutContext : DbContext
    {
        public ArmutContext(DbContextOptions<ArmutContext> options)
            : base(options)
        {
            Database.AutoTransactionsEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<EventEntity> EventEntities { get; set; }
    }
}
