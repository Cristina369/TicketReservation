using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Models.DTO
{
    public class AddSeatRequestDto
    {
        public string SelectedLocation { get; set; }
        public string SvgFilePath { get; set; }

    }
}
