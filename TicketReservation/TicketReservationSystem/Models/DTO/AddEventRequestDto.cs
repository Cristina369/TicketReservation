namespace TicketReservationSystem.Models.DTO
{
    public class AddEventRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventDate { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string PriceRangeEvent { get; set; }
        public string PriceRangeForTickets { get; set; }
        public string EventType { get; set; }
        public string ImageEvent { get; set; }
        public string SelectedLocation { get; set; }
    }
}
