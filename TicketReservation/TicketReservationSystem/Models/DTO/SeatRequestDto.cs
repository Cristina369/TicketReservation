using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Models.DTO
{
    public class SeatRequestDto
    {
        public Guid Id { get; set; }
        public string Row { get; set; }
        public string Zone { get; set; }
        public string Number { get; set; }
        public bool IsAvailable { get; set; }
        public string LocationName { get; set; }
        public Guid? LocationId { get; set; }
    }
}
