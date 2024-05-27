using Microsoft.EntityFrameworkCore;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public LocationRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Location?> AddASync(Location location)
        {
            await applicationDbContext.AddAsync(location);
            await applicationDbContext.SaveChangesAsync();

            return location;
        }

        public async Task<Location?> DeleteASync(Guid id)
        {
            var existingLocation = await applicationDbContext.Locations.FindAsync(id);

            if(existingLocation != null)
            {
                applicationDbContext.Locations.Remove(existingLocation);
                await applicationDbContext.SaveChangesAsync();

                return existingLocation;
            }
            return null;
        }

        public async Task<IEnumerable<Location>> GetAllASync()
        {
            return await applicationDbContext.Locations.ToListAsync();
        }

        public async Task<Location?> GetASync(Guid id)
        {
            return await applicationDbContext.Locations.FirstAsync(x => x.LocationId == id);
        }

        public async Task<Location?> UpdateASync(Location location)
        {
           var existingLocation = await applicationDbContext.Locations.FirstOrDefaultAsync(l  => l.LocationId == location.LocationId);

            if(existingLocation != null)
            {
                existingLocation.Name = location.Name;
                existingLocation.Type = location.Type;
                existingLocation.Capacity = location.Capacity;
                existingLocation.Address = location.Address;
                existingLocation.Description = location.Description;

                await applicationDbContext.SaveChangesAsync();
                return existingLocation;
            }
            return null;
        }
    }
}
