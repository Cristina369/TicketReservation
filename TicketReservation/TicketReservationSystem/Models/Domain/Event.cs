namespace TicketReservationSystem.Models.Domain
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Prices { get; set; }
        public string PricesEvent { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public string ImageEvent { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
