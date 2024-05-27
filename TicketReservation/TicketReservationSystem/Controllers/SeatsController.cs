using Microsoft.AspNetCore.Mvc;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Models.DTO;
using TicketReservationSystem.Repositories.Interface;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatRepository seatRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IEventRepository eventRepository;
        private readonly ITicketRepository ticketRepository;

        public SeatsController(ISeatRepository seatRepository,
            ILocationRepository locationRepository,
            IEventRepository eventRepository,
            ITicketRepository ticketRepository)
        {
            this.seatRepository = seatRepository;
            this.locationRepository = locationRepository;
            this.eventRepository = eventRepository;
            this.ticketRepository = ticketRepository;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Add(AddSingleSeat addSingleSeat)
        {
            if (addSingleSeat == null)
            {
                return BadRequest("Invalid data.");
            }

            var seat = new Seat
            {
                Row = addSingleSeat.Row,
                Number = addSingleSeat.Number,
                Zone = addSingleSeat.Zone,
                IsAvailable = true
            };

            if (!Guid.TryParse(addSingleSeat.SelectedLocation, out Guid selectedLocationGuid))
            {
                return BadRequest("Invalid selected location ID format.");
            }

            var existingLocation = await locationRepository.GetASync(selectedLocationGuid);

            if (existingLocation != null)
            {
                seat.LocationId = existingLocation.LocationId;
            }
            else
            {
                return BadRequest("Selected location not found.");
            }

            try
            {
                await seatRepository.AddAsync(seat);

                var response = new AddSingleSeat
                {
                    Id = seat.SeatId,
                    Row = seat.Row,
                    Number = seat.Number,
                    Zone = seat.Zone,
                    IsAvailable = seat.IsAvailable,
                    SelectedLocation = seat.LocationId.ToString()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the model.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var seats = await seatRepository.GetAllAsync();
                var locations = await locationRepository.GetAllASync();

                var response = new List<SeatRequestDto>();
                foreach (var seat in seats)
                {
                    var location = locations.FirstOrDefault(m => m.LocationId == seat.LocationId);
                    var locationName = location != null ? location.Name : "Unknown";

                    response.Add(new SeatRequestDto
                    {
                        Id = seat.SeatId,
                        Row = seat.Row,
                        Zone = seat.Zone,
                        Number = seat.Number,
                        IsAvailable = seat.IsAvailable,
                        LocationId = location.LocationId,
                        LocationName = locationName
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
    }
}

        [HttpGet("GetSeatsByLocation")]
        public async Task<IActionResult> ListByLocation(Guid id)
        {
            try { 
            var seats = await seatRepository.GetAllAsync();
            var location = await locationRepository.GetASync(id);

            var response = new List<SeatRequestDto>();
            foreach (var seat in seats)
            {
                var locationName = location != null ? location.Name : "Unknown";

                response.Add(new SeatRequestDto
                {
                    Id = seat.SeatId,
                    Row = seat.Row,
                    Number = seat.Number,
                    Zone = seat.Zone,
                    IsAvailable = seat.IsAvailable,
                    LocationId = location.LocationId,
                    LocationName = locationName
                });
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetSeatsByEventAndLocation")]
        public async Task<IActionResult> ListByEvent([FromQuery] Guid eventId)
        {
            try { 
            var seats = await seatRepository.GetAllAsync();
            var eventM = await eventRepository.GetAsync(eventId);

            var response = new List<SeatRequestDto>();
            foreach (var seat in seats)
            {
                var ticket = await ticketRepository.GetByEventAndSeatIdAsync(eventId, seat.SeatId);
                //bool isAvailable = ticket != null && ticket.IsAvailable;

                if (ticket != null)
                {
                    response.Add(new SeatRequestDto
                    {
                        Id = seat.SeatId,
                        Row = seat.Row,
                        Number = seat.Number,
                        Zone = seat.Zone,
                        IsAvailable = ticket.IsAvailable,
                        LocationId = seat.LocationId
                    });
                }
            }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpGet("GetSeatById")]
        public async Task<IActionResult> SeatById(Guid id)
        {
            try { 
            var seat = await seatRepository.GetAsync(id);
            var locations = await locationRepository.GetAllASync();

            var location = locations.FirstOrDefault(m => m.LocationId == seat.LocationId);
            var locationName = location != null ? location.Name : "Unknown";

            var response = new SeatRequestDto
            {
                Id = seat.SeatId,
                Row = seat.Row,
                Number = seat.Number,
                Zone = seat.Zone,
                IsAvailable = seat.IsAvailable,
                LocationId = location.LocationId,
                LocationName = locationName
            };
            return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetSeatByPosition")]
        public async Task<IActionResult> GetSeatByPosition(string row, string zone, string number, string eventId)
        {
            try { 
            var eventM = await eventRepository.GetAsync(Guid.Parse(eventId));

            if (eventM == null)
            {
                return NotFound($"Event not found with ID {eventId}.");
            }

            var location = await locationRepository.GetASync(eventM.LocationId);
            if (location == null)
            {
                return NotFound($"Location not found for event with ID {eventId}.");
            }

            var seat = await seatRepository.GetSeatByPositionAsync(row, zone, number, location.LocationId);
            if (seat == null)
            {
                return NotFound($"Seat not found for {row}, {zone}, {number} at location {location.Name}.");
            }

            return Ok(seat.SeatId);
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
                var seat = await seatRepository.GetAsync(id);
                var locations = await locationRepository.GetAllASync();

                var location = locations.FirstOrDefault(m => m.LocationId == seat.LocationId);
                var locationName = location != null ? location.Name : "Unknown";

                if (seat != null)
                {
                    var response = new SeatRequestDto
                    {
                        Id = seat.SeatId,
                        Row = seat.Row,
                        Number = seat.Number,
                        Zone = seat.Zone,
                        IsAvailable = seat.IsAvailable,
                        LocationId = location.LocationId,
                        LocationName = locationName
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

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, EditSeatRequestDto editSeatRequest)
        {
            try { 
            if(editSeatRequest == null)
            {
                return BadRequest("Invalid model data");
            }

            var seat = new Seat
            {
                SeatId = id,
                Zone = editSeatRequest.Zone,
                Row = editSeatRequest.Row,
                Number = editSeatRequest.Number,
                IsAvailable = editSeatRequest.IsAvailable,
            };

            var selectedLocationIdGuid = Guid.Parse(editSeatRequest.LocationId);
            var existingLocation = await locationRepository.GetASync(selectedLocationIdGuid);

            if (existingLocation != null)
            {
                seat.LocationId = existingLocation.LocationId;
            }

            var updatedSeat = await seatRepository.UpdateAsync(seat);

            if(updatedSeat == null)
            {
                return NotFound();
            }

            var response = new Seat
            {
                SeatId = seat.SeatId,
                Zone = seat.Zone,
                Row = seat.Row,
                Number = seat.Number,
                IsAvailable = seat.IsAvailable,
                LocationId = seat.LocationId
           
            };
            return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }

        [HttpPost("PopulateSeats")]
        public async Task<IActionResult> PopulateSeats(AddSeatRequestDto addSeatRequest)
        {
            try { 
            if (addSeatRequest == null)
            {
                return BadRequest("Invalid data.");
            }

            IEnumerable<Seat> seats = ParseSVGSeats("../../../CRSystem.UI/CinemaReservationSystem/src/assets/" + addSeatRequest.SvgFilePath + ".svg");

            foreach (var seat in seats)
            {
                seat.LocationId = Guid.Parse(addSeatRequest.SelectedLocation);
                await seatRepository.AddAsync(seat);
            }

            return Ok("Seats populated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        private IEnumerable<Seat> ParseSVGSeats(string svgFilePath)
        {
            var seats = new List<Seat>();

            var doc = new HtmlDocument();
            doc.Load(svgFilePath);

            var seatNodes = doc.DocumentNode.SelectNodes("//circle");

            if (seatNodes != null)
            {
                foreach (var seatNode in seatNodes)
                {
                    var zone = seatNode.GetAttributeValue("zone", "");
                    var row = seatNode.GetAttributeValue("row", "");
                    var number = seatNode.GetAttributeValue("number", "");

                    var seat = new Seat
                    {
                        SeatId = Guid.NewGuid(),
                        Zone = zone,
                        Row = row,
                        Number = number,
                        IsAvailable = true,
                    };

                    seats.Add(seat);
                }
            }

            return seats;
        }
    }
}
