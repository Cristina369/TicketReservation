using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface IEventRepository
    {
        Task<Event?> GetAsync(Guid id);
        Task<Event?> AddAsync(Event eventM);
        Task<Event> UpdateAsync(Event eventM);
        Task<Event?> DeleteAsync(Guid id);
        Task<IEnumerable<Event>> GetAllAsync();
    }
}
