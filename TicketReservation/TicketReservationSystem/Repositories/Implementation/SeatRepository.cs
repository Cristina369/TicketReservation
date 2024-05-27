using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SeatRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Seat?> AddAsync(Seat seat)
        {
            await applicationDbContext.AddAsync(seat);
            await applicationDbContext.SaveChangesAsync();

            return seat;
        }

        public async Task<Seat?> DeleteAsync(Guid id)
        {
            var existingSeat = await applicationDbContext.Seats.FindAsync(id);

            if (existingSeat != null)
            {
                applicationDbContext.Seats.Remove(existingSeat);
                await applicationDbContext.SaveChangesAsync();
                return existingSeat;
            }

            return null;
        }

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            return await applicationDbContext.Seats.Include(c => c.Ticket).Include(c => c.Location).ToListAsync();

        }

        public async Task<Seat?> GetAsync(Guid id)
        {
            return await applicationDbContext.Seats
                .Include(c => c.Ticket).Include(c => c.Location)
                .FirstAsync(c => c.SeatId == id);
        }
        public async Task<Seat> ReserveSeat(Guid seatId, Guid locationId)
        {
           var seat = await applicationDbContext.Seats
                .Include(s => s.Ticket).Include(s => s.Location)
                .FirstOrDefaultAsync(s => s.SeatId == seatId && s.LocationId == locationId );
            
            if (seat != null && seat.IsAvailable)
            {
                seat.IsAvailable = false;

                seat.LocationId = locationId;
                await applicationDbContext.SaveChangesAsync();

            }
            return seat;
         }

        public async Task<IEnumerable<Seat>> GetSeatsByLocationIdAsync(Guid locationId)
        {
            return await applicationDbContext.Seats
                .Where(s => s.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<Seat?> UpdateAsync(Seat seat)
        {
            var existingSeat = await applicationDbContext.Seats.FindAsync(seat.SeatId);

            if (existingSeat != null)
            {
                existingSeat.SeatId = seat.SeatId;
                existingSeat.Row = seat.Row;
                existingSeat.Number = seat.Number;
                existingSeat.Zone = seat.Zone;
                existingSeat.IsAvailable = seat.IsAvailable;
                existingSeat.LocationId = seat.LocationId;
                existingSeat.TicketId = seat.TicketId;

                await applicationDbContext.SaveChangesAsync();
                return existingSeat;
            }

            return null;
        }

        public async Task<Seat> GetSeatByPositionAsync(string row, string zone, string number, Guid locationId)
        {
            return await applicationDbContext.Seats.FirstOrDefaultAsync(s =>
                s.Row == row &&
                s.Zone == zone &&
                s.Number == number &&
                s.LocationId == locationId);
        }


    }
}
