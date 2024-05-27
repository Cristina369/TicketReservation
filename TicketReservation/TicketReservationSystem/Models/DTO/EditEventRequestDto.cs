namespace TicketReservationSystem.Models.DTO
{
    public class EditEventRequestDto
    {
        public string Name { get; set; }
        public string Prices { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string EventType { get; set; }
        public string ImageEvent { get; set; }

        public string SelectedLocation { get; set; }
    }
}
