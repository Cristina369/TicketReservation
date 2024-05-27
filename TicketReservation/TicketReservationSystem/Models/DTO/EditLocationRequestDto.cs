namespace TicketReservationSystem.Models.DTO
{
    public class EditLocationRequestDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string LocationPath { get; set; }
    }
}
