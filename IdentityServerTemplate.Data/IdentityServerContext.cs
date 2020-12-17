using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Data
{
    public class IdentityServerContext : IdentityDbContext<Account, Profile, Guid>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profiles { get; set; }        

        public IdentityServerContext(DbContextOptions<IdentityServerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
                property.SetColumnType("decimal(18, 2)");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().
                                    GetProperty(nameof(Entity<Guid>.CreatedAt)) != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(Entity<Guid>.CreatedAt)).CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(Entity<Guid>.UpdatedAt)).CurrentValue = DateTime.Now;
                }
            }

            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }
}
