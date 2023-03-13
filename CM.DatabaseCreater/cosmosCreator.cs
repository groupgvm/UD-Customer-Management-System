using CM.DatabaseCreater;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CM.DatabaseCreater
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
            );
            _database = _client.GetDatabase(_configuration["Configurations:DbName"]);

        }

        public async Task CreateContainers()
        {
            var containers = new List<(string id, string partitionKeyPath, int throughput)>();

            // Add new containers here
            containers.Add(("Customer", "/id", 400));

            foreach(var container in containers)
            {
                try
                {
                    await _database.CreateContainerIfNotExistsAsync(container.id, container.partitionKeyPath, container.throughput);
                    Console.WriteLine("Customer container created successfully.\n");

                }
                catch (Exception ex)
                {
                    throw new Exception("Error in creating container: " + container.id);
                }
            }
        }
  

    }

}





