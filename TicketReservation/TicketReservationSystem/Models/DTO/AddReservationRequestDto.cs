using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Models.DTO
{
    public class AddReservationRequestDto
    {
        public string PaymentMethod { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string ClientAddress { get; set; }
        public Guid Event { get; set; }
        public ICollection<Guid> Seats { get; set; }
    }
}
