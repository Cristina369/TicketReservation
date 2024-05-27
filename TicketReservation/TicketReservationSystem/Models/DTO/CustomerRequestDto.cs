using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Models.DTO
{
    public class CustomerRequestDto
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

    }
}
