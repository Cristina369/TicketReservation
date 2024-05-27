namespace TicketReservationSystem.Models.Domain
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public string LocationPath { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Event> Events { get; set;}
    }
}
