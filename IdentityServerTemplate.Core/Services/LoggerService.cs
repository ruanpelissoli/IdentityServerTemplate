using IdentityServerTemplate.Shared.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Services
{
    public class LoggerService : ILoggerService
    {
        public readonly IIdentityUserService _identityUserService;

        private readonly CosmosClient cosmosClient;
        private Database _database;
        private Container _infoContainer;
        private Container _exceptionContainer;

        private readonly string _databaseId;
        private readonly string _infoContainerId;
        private readonly string _exceptionContainerId;

        public LoggerService(IIdentityUserService identityUserService, IConfiguration configuration)
        {
            _identityUserService = identityUserService;

            _databaseId = configuration.GetValue<string>("CosmosDb:DatabaseId"); //TODO: refactor magic strings
            _infoContainerId = configuration.GetValue<string>("CosmosDb:InfoContaier");
            _exceptionContainerId = configuration.GetValue<string>("CosmosDb:ExceptionsContainer");

            cosmosClient = new CosmosClient(
                configuration.GetValue<string>("CosmosDb:EndpointUri"),
                configuration.GetValue<string>("CosmosDb:PrimaryKey"),
                new CosmosClientOptions() { ApplicationName = "Esfer" });

            Task.Run(async () =>
            {
                _database = await cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
                _infoContainer = await _database.CreateContainerIfNotExistsAsync(_infoContainerId, "/UserId", 400);
                _exceptionContainer = await _database.CreateContainerIfNotExistsAsync(_exceptionContainerId, "/UserId", 400);
            }).Wait();
        }

        public async Task Error(string error)
            => await _exceptionContainer.CreateItemAsync(error, new PartitionKey(_identityUserService.Id.ToString()));

        public async Task Info(object message)
            => await _infoContainer.CreateItemAsync(message, new PartitionKey(_identityUserService.Id.ToString()));
    }
}

