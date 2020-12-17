using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Core.IdentityServerConfig;
using IdentityServerTemplate.Core.Repositories;
using IdentityServerTemplate.Core.Services;
using IdentityServerTemplate.Data.Repositories;
using IdentityServerTemplate.Data.Seeds;
using IdentityServerTemplate.Shared.Consts;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdentityServerTemplate.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("IdentityServerDatabase");
            var migrationAssembly = typeof(IdentityServerContext).GetTypeInfo().Assembly.GetName().Name;
            var identityServerUrl = configuration.GetSection("IdentityServer").Value;

            services.AddScoped<IAccountRepository, AccountRepository>();            

            services.AddIdentity<Account, Profile>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<IdentityServerContext>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        if (environment.EnvironmentName.Equals(Environments.DEV))
                        {
                            options.RequireHttpsMetadata = false;
                        }

                        options.Authority = identityServerUrl;
                        options.ApiName = IdentityServerConfig.API_NAME;
                    });

            services.AddDbContext<IdentityServerContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging(environment.EnvironmentName.Equals(Environments.DEV));
            });

            services.AddIdentityServer(x => { x.IssuerUri = identityServerUrl; })
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationAssembly));

                    options.EnableTokenCleanup = false;
                    options.TokenCleanupInterval = 30;
                })
                .AddAspNetIdentity<Account>()
                .AddProfileService<IdentityProfileService>();

            return services;
        }

        public static IApplicationBuilder BuildIdentityServer(this IApplicationBuilder app)
        {
            InitializeDatabase.Up(app);

            return app.UseAuthentication()
                      .UseIdentityServer()
                      .UseAuthorization();
        }
    }
}
