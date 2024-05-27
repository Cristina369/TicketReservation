using Microsoft.EntityFrameworkCore;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TicketRepository( ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            await applicationDbContext.Tickets.AddAsync(ticket);
            await applicationDbContext.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> GetByEventAndSeatIdAsync(Guid eventId, Guid seatId)
        {
            return await applicationDbContext.Tickets
                .FirstOrDefaultAsync(ticket => ticket.EventId == eventId && ticket.SeatId == seatId);
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            applicationDbContext.Tickets.Update(ticket);
            await applicationDbContext.SaveChangesAsync();


            return ticket;
        }

    }
}
