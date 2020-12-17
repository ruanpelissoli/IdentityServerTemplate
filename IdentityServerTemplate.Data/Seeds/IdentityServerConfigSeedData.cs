using IdentityServerTemplate.Core.IdentityServerConfig;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;

namespace IdentityServerTemplate.Data.Seeds
{
    public static class IdentityServerConfigSeedData
    {
        public static void EnsureSeedData(this ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in IdentityServerConfig.GetClients())
                    context.Clients.Add(client.ToEntity());

                context.SaveChanges();

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in IdentityServerConfig.GetApiScopes())
                        context.ApiScopes.Add(scope.ToEntity());
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.GetIdentityResources())
                        context.IdentityResources.Add(resource.ToEntity());

                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.GetApis())
                        context.ApiResources.Add(resource.ToEntity());

                    context.SaveChanges();
                }
            }
        }
    }
}
