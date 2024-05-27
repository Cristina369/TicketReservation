using Microsoft.AspNetCore.Mvc.Rendering;
using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Models.DTO
{
    public class EditSeatRequestDto
    {
        public string Row { get; set; }
        public string Number { get; set; }
        public string Zone { get; set; }
        public bool IsAvailable { get; set; }
        public string LocationId { get; set; }

    }
}
