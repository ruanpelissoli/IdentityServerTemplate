using Azure.Storage.Queues;
using IdentityServerTemplate.Core.Consts;
using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Shared.Extensions;
using IdentityServerTemplate.Shared.Services;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Services
{
    public class ForgotPasswordEmailQueueService : IStorageQueue<ForgotPasswordEmailDTO>
    {
        private readonly IConfiguration _configuration;

        private readonly QueueClient _queueClient;

        public ForgotPasswordEmailQueueService(IConfiguration configuration)
        {
            _configuration = configuration;

            _queueClient = new QueueClient(_configuration.GetValue<string>("AzureStorage"), StorageQueues.FORGOT_PASSWORD_EMAIL);
            _queueClient.CreateIfNotExists();
        }

        public async Task Enqueue(ForgotPasswordEmailDTO obj, CancellationToken ct)
        {
            if (_queueClient.Exists())            
                await _queueClient.SendMessageAsync(obj.ToBase64String(), ct);
        }
    }
}
