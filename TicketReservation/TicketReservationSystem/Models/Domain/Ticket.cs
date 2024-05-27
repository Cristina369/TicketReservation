namespace TicketReservationSystem.Models.Domain
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public Guid? SeatId { get; set; }
        public Seat? Seat { get; set; }

        public Guid? ReservationId { get; set; }
        public Reservation Reservation { get; set; }

    }
}
