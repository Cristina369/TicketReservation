using System.ComponentModel.DataAnnotations.Schema;

namespace TicketReservationSystem.Models.Domain
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNumber { get; set; }
        public int AmountOfTickets { get; set; }
        public double TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }        
        public ICollection<Ticket> Tickets { get; set;}
    }
}
