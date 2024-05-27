using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EventRepository( ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Event> AddAsync(Event eventM)
        {
            await applicationDbContext.Events.AddAsync(eventM);
            await applicationDbContext.SaveChangesAsync();

            return eventM;
        }

        public async Task<Event?> DeleteAsync(Guid id)
        {
            var eventM = await applicationDbContext.Events
                    .Include(e => e.Tickets)
                    .ThenInclude(t => t.Reservation)
                    .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventM != null)
            {
                var reservationsToDelete = eventM.Tickets
                    .Where(t => t.Reservation != null)
                    .Select(t => t.Reservation)
                    .Distinct()
                    .ToList();

                applicationDbContext.Tickets.RemoveRange(eventM.Tickets);

                applicationDbContext.Reservations.RemoveRange(reservationsToDelete);

                applicationDbContext.Events.Remove(eventM);

                await applicationDbContext.SaveChangesAsync();

                return eventM;
            }

            return null;
        }


        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await applicationDbContext.Events.ToListAsync();
        }

        public async Task<Event?> GetAsync(Guid id)
        {
            return await applicationDbContext.Events
                .SingleOrDefaultAsync(e => e.EventId == id);
        }

        public async Task<Event> UpdateAsync(Event eventM)
        {
            var existingEvent = await applicationDbContext.Events.FirstOrDefaultAsync(e => e.EventId == eventM.EventId);

            if(existingEvent != null )
            {
                existingEvent.Name = eventM.Name;
                existingEvent.Description = eventM.Description;
                existingEvent.Prices = eventM.Prices;
                existingEvent.EventDate = eventM.EventDate;
                existingEvent.EventType = eventM.EventType;
                existingEvent.ImageEvent = eventM.ImageEvent;
                existingEvent.StartTime = eventM.StartTime;
                existingEvent.FinishTime = eventM.FinishTime;
                existingEvent.LocationId = eventM.LocationId;

                await applicationDbContext.SaveChangesAsync();

                return existingEvent;
            }

            return null;
        }
    }
}
