using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Services;
using IdentityServerTemplate.Core.Services.FakeServices;
using IdentityServerTemplate.Shared.Consts;
using IdentityServerTemplate.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IdentityServerTemplate.Core.Factories
{
    public static class ForgotPasswordEmailQueueFactory
    {
        public static IStorageQueue<ForgotPasswordEmailDTO> CreateQueueService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.EnvironmentName.Equals(Environments.DEV))
                return new FakeForgotPasswordEmailQueueService(configuration);

            return new ForgotPasswordEmailQueueService(configuration);
        }
    }
}
