using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Services;
using IdentityServerTemplate.Shared.Consts;
using IdentityServerTemplate.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IdentityServerTemplate.Core.Factories
{
    public static class ConfirmationEmailQueueFactory
    {
        public static IStorageQueue<ConfirmationEmailDTO> CreateQueueService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.EnvironmentName.Equals(Environments.DEV))
                return new FakeConfirmationEmailQueueService(configuration);

            return new ConfirmationEmailQueueService(configuration);
        }
    }
}
