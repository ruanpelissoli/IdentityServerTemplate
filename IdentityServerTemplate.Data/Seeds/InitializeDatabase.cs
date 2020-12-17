using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerTemplate.Data.Seeds
{
    public static class InitializeDatabase
    {
        public static void Up(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            {
                var context = serviceScope.ServiceProvider.GetService<IdentityServerContext>();
                context.Database.Migrate();
            }

            {
                var context = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                context.Database.Migrate();
            }

            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                context.EnsureSeedData();
            }

            {
                var context = serviceScope.ServiceProvider.GetService<IdentityServerContext>();
                context.EnsureSeedData();
            }
        }
    }
}
