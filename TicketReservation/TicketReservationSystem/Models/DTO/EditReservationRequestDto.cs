namespace TicketReservationSystem.Models.DTO
{
    public class EditReservationRequestDto
    {
        public string PaymentMethod { get; set; }
        public string TransactionNumber { get; set; }
        public int AmountOfTickets { get; set; }
        public double TotalPrice { get; set; }

    }
}
