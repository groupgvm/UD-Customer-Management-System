using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;

namespace CM.Common.Services
{
    public class CosmosService
    {
        private readonly CosmosClient _client;
        private readonly IConfiguration _configuration;
        private readonly Database _database;

        public CosmosService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new CosmosClient(
                connectionString: _configuration["Configurations:CustomerCosmosDBString"]
            ) ;
            _database = _client.GetDatabase(_configuration["Configurations:DbName"]);
            
        }


        // Containers
        public Container Customer
        {
            get => _database.GetContainer("Customer");
        }

 
    }
}