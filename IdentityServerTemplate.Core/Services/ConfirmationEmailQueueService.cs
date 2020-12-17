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
    public class ConfirmationEmailQueueService : IStorageQueue<ConfirmationEmailDTO>
    {
        private readonly IConfiguration _configuration;

        private readonly QueueClient _queueClient;

        public ConfirmationEmailQueueService(IConfiguration configuration)
        {
            _configuration = configuration;

            _queueClient = new QueueClient(_configuration.GetValue<string>("AzureStorage"), StorageQueues.CONFIRMATION_EMAIL);
            _queueClient.CreateIfNotExists();
        }

        public async Task Enqueue(ConfirmationEmailDTO obj, CancellationToken ct)
        {
            if (_queueClient.Exists())            
                await _queueClient.SendMessageAsync(obj.ToBase64String(), ct);
        }
    }
}
