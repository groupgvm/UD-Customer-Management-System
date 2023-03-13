using CM.Domain.Models;

namespace CM.Domain
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();

        Task AddCustomerAsync(Customer customer);
    }
}