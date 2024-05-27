using System.Net.Mail;
using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Repositories.Interface
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation?> GetAsync(Guid id);
        Task<Reservation?> AddAsync(Reservation reservation);
        Task<Reservation?> UpdateAsync(Reservation reservation);
        Task<Reservation?> DeleteAsync(Guid id);
        Task SendEmailWithAttachment(string recipientEmail, string subject, string body, List<Attachment> attachments);
    }
}
