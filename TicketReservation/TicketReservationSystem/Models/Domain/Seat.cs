namespace TicketReservationSystem.Models.Domain
{
    public class Seat
    {
        public Guid SeatId { get; set; }
        public string Zone { get; set; }
        public string Row { get; set; }
        public string Number { get; set; }
        public bool IsAvailable { get; set; } 
        public Guid? LocationId { get; set; }
        public Location? Location { get; set; }

        public Guid? TicketId { get; set; }
        public Ticket? Ticket { get; set; }

    }
}
