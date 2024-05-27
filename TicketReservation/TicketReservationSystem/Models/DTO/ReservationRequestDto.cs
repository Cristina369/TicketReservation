namespace TicketReservationSystem.Models.DTO
{
    public class ReservationRequestDto
    {
        public Guid ReservationId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNumber { get; set; }
        public int AmountOfTickets { get; set; }
        public double TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
