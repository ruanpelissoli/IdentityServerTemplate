using IdentityServerTemplate.Core.AutoMapper;
using IdentityServerTemplate.Core.Factories;
using IdentityServerTemplate.Core.Services;
using IdentityServerTemplate.LanguageResources;
using IdentityServerTemplate.Shared.Email;
using IdentityServerTemplate.Shared.Services;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdentityServerTemplate.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<SMTPSettings>(configuration.GetSection("SMTPSettings"));

            services.AddLocalizationDependencies();

            services.AddScoped<ILoggerService, LoggerService>();

            services.AddScoped<IProfileService, IdentityProfileService>();
            services.AddScoped<IIdentityUserService, IdentityUserService>();
            services.AddScoped(x =>
                ConfirmationEmailQueueFactory.CreateQueueService(configuration, environment));
            services.AddScoped(x =>
               ForgotPasswordEmailQueueFactory.CreateQueueService(configuration, environment));
            services.AddMediatR(Assembly.GetAssembly(typeof(ServiceCollectionExtensions)));

            AutoMapperConfiguration.Execute(services);

            return services;
        }

        public static IApplicationBuilder UseCoreDependencies(this IApplicationBuilder app) =>
           app.UseRequestLocalization();
    }
}
