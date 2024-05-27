using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetAsync(Guid id);
        Task<Customer?> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(Customer customer);
        Task<Customer?> DeleteAsync(Guid id);
    }
}
