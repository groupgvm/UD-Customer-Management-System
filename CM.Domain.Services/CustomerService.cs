using CM.Common.Services;
using CM.Domain.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CM.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CosmosService _db;

        public CustomerService(CosmosService db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var queryable = _db.Customer.GetItemLinqQueryable<CM.DbModel.Customer>();
            using FeedIterator<CM.DbModel.Customer> feed = queryable.ToFeedIterator();
           
            List<Customer> results = new();
            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                foreach (CM.DbModel.Customer item in response)
                {
                    results.Add(new Customer
                    {
                        Address = item.Address,
                        Email = item.Email,
                        Id = item.Id,
                        Name = item.Name,
                        Phone = item.Phone
                    });
                }
            }

            return results;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}