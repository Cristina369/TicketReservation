using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ReservationRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Reservation?> AddAsync(Reservation reservation)
        {
            await applicationDbContext.AddAsync(reservation);
            await applicationDbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation?> DeleteAsync(Guid id)
        {
            try
            {
                var existingReservation = await applicationDbContext.Reservations
                    .Include(r => r.Tickets) 
                    .FirstOrDefaultAsync(r => r.ReservationId == id);

                if (existingReservation != null)
                {
                    applicationDbContext.Tickets.RemoveRange(existingReservation.Tickets);
                    applicationDbContext.Reservations.Remove(existingReservation);
                    await applicationDbContext.SaveChangesAsync();

                    return existingReservation;
                }
                return null;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DbUpdateException error: {dbEx.Message}");
                throw; 
            }

        }



        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await applicationDbContext.Reservations.ToListAsync();
        }

        public async Task<Reservation?> GetAsync(Guid id)
        {
            return await applicationDbContext.Reservations.FirstAsync(c => c.ReservationId == id);
        }

        public async Task<Reservation?> UpdateAsync(Reservation reservation)
        {
            var existingReservation = await applicationDbContext.Reservations.Include(r => r.CustomerId).FirstOrDefaultAsync(r => r.ReservationId == reservation.ReservationId);
            
            if(existingReservation != null)
            {
                existingReservation.TransactionNumber = reservation.TransactionNumber;
                existingReservation.PaymentMethod = reservation.PaymentMethod;
                existingReservation.AmountOfTickets = reservation.AmountOfTickets;
                existingReservation.TotalPrice = reservation.TotalPrice;
                existingReservation.CustomerId = reservation.CustomerId;

                await applicationDbContext.SaveChangesAsync();
                return existingReservation;
            }
            return null;
        }

        public async Task SendEmailWithAttachment(string recipientEmail, string subject, string body, List<Attachment> attachments)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("", "E-Tickets");
                    mailMessage.To.Add(new MailAddress(recipientEmail, subject));
                    mailMessage.Subject = "Reservation Ticket";
                    mailMessage.Body = body;

                    foreach (var attachment in attachments)
                    {
                        mailMessage.Attachments.Add(attachment);
                    }

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
