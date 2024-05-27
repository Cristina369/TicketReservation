namespace TicketReservationSystem.Models.DTO
{
    public class EventByIdRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string EventType { get; set; }
        public string ImageEvent { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string SeatMapPath { get; set; }
    }
}
