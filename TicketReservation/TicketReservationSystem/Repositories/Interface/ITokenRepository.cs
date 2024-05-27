using Microsoft.AspNetCore.Identity;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
