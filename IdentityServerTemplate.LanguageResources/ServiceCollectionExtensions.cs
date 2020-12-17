using IdentityServerTemplate.LanguageResources.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace IdentityServerTemplate.LanguageResources
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizationDependencies(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(
            opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("pt-BR"),
                        new CultureInfo("en-US")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
            services.AddScoped<IApiMessagesResource, ApiMessagesResource>();

            return services;
        }

        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app) =>
            app.UseRequestLocalization();
    }
}
