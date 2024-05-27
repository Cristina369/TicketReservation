using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllAsync();
        Task<Seat?> GetAsync(Guid id);
        Task<Seat> ReserveSeat(Guid seatId, Guid locationId);
        Task<Seat?> AddAsync(Seat seat);
        Task<Seat?> UpdateAsync(Seat seat);
        Task<Seat?> DeleteAsync(Guid id);
        Task<IEnumerable<Seat?>> GetSeatsByLocationIdAsync(Guid locationId);
        Task<Seat> GetSeatByPositionAsync(string row, string zone, string number, Guid locationId);
       
    }
}
