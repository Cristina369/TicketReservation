using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllASync();
        Task<Location?> AddASync(Location location);
        Task<Location?> GetASync(Guid id);
        Task<Location?> UpdateASync(Location location);
        Task<Location?> DeleteASync(Guid id);
    }
}
