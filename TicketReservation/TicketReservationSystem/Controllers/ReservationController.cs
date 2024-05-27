using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System.Drawing;
using System.Net.Mail;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Models.DTO;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository reservationRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ISeatRepository seatRepository;
        private readonly IEventRepository eventRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ITicketRepository ticketRepository;

        private static readonly Random _random = new Random();


        public ReservationController(IReservationRepository reservationRepository,
            ICustomerRepository customerRepository,
            ISeatRepository seatRepository, 
            IEventRepository eventRepository, 
            ILocationRepository locationRepository, ITicketRepository ticketRepository)
        {
            this.reservationRepository = reservationRepository;
            this.customerRepository = customerRepository;
            this.seatRepository = seatRepository;
            this.eventRepository = eventRepository;
            this.locationRepository = locationRepository;
            this.ticketRepository = ticketRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddReservationRequestDto addReservationRequest)
        {
            if (addReservationRequest == null)
            {
                return BadRequest("Invalid model data.");
            }

            if (addReservationRequest.Seats.Count > 5)
            {
                return BadRequest("You can reserve up to 5 seats per reservation.");
            }

            var eventM = await eventRepository.GetAsync(addReservationRequest.Event);
            if (eventM == null)
            {
                return NotFound("Event not found");
            }
   

            var client = new Customer
            {
                FirstName = addReservationRequest.ClientFirstName,
                LastName = addReservationRequest.ClientLastName,
                Email = addReservationRequest.ClientEmail,
                Phone = addReservationRequest.ClientPhone,
                Address = addReservationRequest.ClientAddress
            };

            var reservedSeats = new List<Seat>();
            foreach (var seatId in addReservationRequest.Seats)
            {
                var seat = await seatRepository.GetAsync(seatId);
                if (seat == null)
                {
                    return NotFound($"Seat with ID {seatId} not found.");
                }
                if (!seat.IsAvailable)
                {
                    return BadRequest($"Seat with ID {seatId} is not available for reservation.");
                }
                
               reservedSeats.Add(seat);
            }

            var tickets = new List<Ticket>();
            var priceTotalTickets = 0;
            foreach (var seat in reservedSeats)
            {
                var ticket = await ticketRepository.GetByEventAndSeatIdAsync(addReservationRequest.Event, seat.SeatId);
                if (ticket == null)
                {
                    return NotFound($"Ticket for seat with ID {seat.SeatId} not found.");
                }
                ticket.IsAvailable = false;
                priceTotalTickets += ticket.Price; 
                await ticketRepository.UpdateAsync(ticket);
                tickets.Add(ticket);
            }


            await customerRepository.AddAsync(client);
            string transactionNumber = GenerateTransactionNumber();


            var reservation = new Reservation
            {
                ReservationId = Guid.NewGuid(),
                PaymentMethod = addReservationRequest.PaymentMethod,
                TransactionNumber = transactionNumber,
                TotalPrice = priceTotalTickets,
                AmountOfTickets = tickets.Count,
                CustomerId = client.CustomerId,
                Tickets = tickets
            };

           await reservationRepository.AddAsync(reservation);


            var response = new Reservation
            {
                ReservationId = reservation.ReservationId,
                PaymentMethod = reservation.PaymentMethod,
                TransactionNumber = transactionNumber,
                TotalPrice = reservation.TotalPrice,
                AmountOfTickets = reservation.AmountOfTickets,
                CustomerId = client.CustomerId,
                Tickets = reservation.Tickets.ToList()
            };


            return Ok(response);

        }


        private string GenerateTransactionNumber()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int randomNumber = _random.Next(1000, 9999);

            string transactionNumber = $"{timestamp}{randomNumber}";

            return transactionNumber;
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var customerDM = await customerRepository.GetAllAsync();
                var reservations = await reservationRepository.GetAllAsync();

                var response = new List<ReservationRequestDto>();
                foreach (var reservation in reservations)
                {
                    var customer = await customerRepository.GetAsync(reservation.CustomerId);
                    var customerFirstName = customer != null ? customer.FirstName : "Unknown";
                    var customerLastName = customer != null ? customer.LastName : "Unknown";
                    var customerEmail = customer != null ? customer.Email : "Unknown";


                    response.Add(new ReservationRequestDto
                    {
                        ReservationId = reservation.ReservationId,
                        PaymentMethod = reservation.PaymentMethod,
                        TransactionNumber = reservation.TransactionNumber,
                        TotalPrice = reservation.TotalPrice,
                        CustomerId = reservation.CustomerId,
                        CustomerName = customerFirstName + customerLastName,
                        CustomerEmail = customerEmail
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
    }
}

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var customerDM = await customerRepository.GetAllAsync();
                var reservation = await reservationRepository.GetAsync(id);

                if (reservation != null)
                {
                    var customer = await customerRepository.GetAsync(reservation.CustomerId);
                    var customerFirstName = customer != null ? customer.FirstName : "Unknown";
                    var customerLastName = customer != null ? customer.LastName : "Unknown";
                    var customerEmail = customer != null ? customer.Email : "Unknown";

                    var response = new ReservationRequestDto
                    {
                        ReservationId = reservation.ReservationId,
                        PaymentMethod = reservation.PaymentMethod,
                        TransactionNumber = reservation.TransactionNumber,
                        TotalPrice = reservation.TotalPrice,
                        CustomerId = reservation.CustomerId,
                        CustomerName = customerFirstName + customerLastName,
                        CustomerEmail = customerEmail
                    };
                    return Ok(response);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
    }
}

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deletedReservation = await reservationRepository.DeleteAsync(id);

                if (deletedReservation == null)
                {
                    return NotFound();
                }

                var response = new ReservationRequestDto
                {
                    ReservationId = deletedReservation.ReservationId,
                    PaymentMethod = deletedReservation.PaymentMethod,
                    TransactionNumber = deletedReservation.TransactionNumber,
                    TotalPrice = deletedReservation.TotalPrice,
                    CustomerId = deletedReservation.CustomerId,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}

