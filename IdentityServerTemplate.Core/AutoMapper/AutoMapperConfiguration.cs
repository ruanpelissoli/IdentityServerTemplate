using AutoMapper;
using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerTemplate.Core.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Execute(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Account, AccountDTO>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}