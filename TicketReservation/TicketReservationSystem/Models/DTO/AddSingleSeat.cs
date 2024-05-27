namespace TicketReservationSystem.Models.DTO
{
    public class AddSingleSeat
    {
        public Guid Id { get; set; }
        public string Row { get; set; }
        public string Number { get; set; }
        public string Zone { get; set; }
        public bool IsAvailable { get; set; }
        public string SelectedLocation { get; set; }
    }
}
