using System.Net.Mail;
using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface ITicketRepository
    {
        Task<Ticket> AddAsync(Ticket ticket);
        Task<Ticket> GetByEventAndSeatIdAsync(Guid eventId, Guid seatId);
        Task<Ticket> UpdateAsync(Ticket ticket);
    }
}
