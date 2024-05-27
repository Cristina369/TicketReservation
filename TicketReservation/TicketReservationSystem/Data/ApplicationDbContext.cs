using Microsoft.EntityFrameworkCore;
using TicketReservationSystem.Models.Domain;

namespace TicketReservationSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EventImage> EventImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Ticket)
                .WithOne(t => t.Seat)
                .HasForeignKey<Ticket>(t => t.SeatId);

            modelBuilder.Entity<Ticket>()
           .HasOne(t => t.Event)
           .WithMany(e => e.Tickets)
           .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Ticket)
                .WithOne(t => t.Seat)
                .HasForeignKey<Seat>(s => s.TicketId)
                .IsRequired(false);
        }

    }
}
