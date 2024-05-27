using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Models.DTO;
using System.Threading.Tasks;
using TicketReservationSystem.Repositories.Interface;
using TicketReservationSystem.Repositories.Implementation;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository eventRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ISeatRepository seatRepository;
        private readonly ITicketRepository ticketRepository;

        public EventsController(IEventRepository eventRepository,
            ILocationRepository locationRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository)
        {
            this.eventRepository = eventRepository;
            this.locationRepository = locationRepository;
            this.seatRepository = seatRepository;
            this.ticketRepository = ticketRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddEventRequestDto addEventRequest)
        {
            if (addEventRequest == null)
            {
                return BadRequest("Invalid data model.");
            }

            var eventM = new Event
            {
                Name = addEventRequest.Name,
                Prices = addEventRequest.PriceRangeForTickets,
                PricesEvent = addEventRequest.PriceRangeEvent,
                Description = addEventRequest.Description,
                EventDate = DateTime.Parse(addEventRequest.EventDate),
                EventType = addEventRequest.EventType,
                ImageEvent = addEventRequest.ImageEvent,
                StartTime = DateTime.Parse(addEventRequest.StartTime),
                FinishTime = DateTime.Parse(addEventRequest.FinishTime)
            };

            if (!Guid.TryParse(addEventRequest.SelectedLocation, out Guid selectedLocationIdGuid))
            {
                return BadRequest("Invalid selected Location ID format.");
            }

            var existingLocation = await locationRepository.GetASync(selectedLocationIdGuid);

            if (existingLocation != null)
            {
                eventM.LocationId = existingLocation.LocationId;
            }
            else
            {
                return BadRequest("Selected Model Or Location not found.");
            }

            eventM.LocationId = existingLocation.LocationId;
            await eventRepository.AddAsync(eventM);

            var seats = await seatRepository.GetSeatsByLocationIdAsync(existingLocation.LocationId);
            string[] prices = addEventRequest.PriceRangeForTickets.Split('.');

            if (prices.Length != 3)
            {
                return BadRequest("Invalid price format. Expected three prices separated by a dot.");
            }

            var tickets = new List<Ticket>();
            foreach (var seat in seats)
            {
                int price = CalculatePriceForZone(seat.Zone, prices);

                var ticket = new Ticket
                {
                    TicketId = Guid.NewGuid(),
                    Name = $"{eventM.Name} - {seat.Zone}", 
                    Details = "", 
                    IsAvailable = true, 
                    Price = price, 
                    EventId = eventM.EventId,
                    SeatId = seat.SeatId,
                    ReservationId = null
                };
                await ticketRepository.AddAsync(ticket);
                tickets.Add(ticket);
            }


            var response = new AddEventRequestDto
            {
                Id = addEventRequest.Id,
                Name = addEventRequest.Name,
                Description = addEventRequest.Description,
                PriceRangeEvent = addEventRequest.PriceRangeEvent,
                EventDate = addEventRequest.EventDate,
                EventType = addEventRequest.EventType,
                ImageEvent = addEventRequest.ImageEvent,
                StartTime = addEventRequest.StartTime,
                FinishTime = addEventRequest.FinishTime,
                SelectedLocation = addEventRequest.SelectedLocation.ToString()
            };

            return Ok(response);

        }

        private int CalculatePriceForZone(string zone, string[] prices)
        {
            switch (zone)
            {
                case "front-zone" or "orchestra-left-zone" or "orchestra-right-zone":
                    return (int)decimal.Parse(prices[2]); 
                case "mezzanine-zone":
                    return (int)decimal.Parse(prices[1]); 
                case "mezzanine-left-zone" or "mezzanine-right-zone":
                    return (int)decimal.Parse(prices[0]); 
                default:
                    return 0; 
            }
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var events = await eventRepository.GetAllAsync();
            var locationDM = await locationRepository.GetAllASync();

            var response = new List<EventRequestDto>();
            foreach (var eventM in events)
            {
                var location = locationDM.FirstOrDefault(m => m.LocationId == eventM.LocationId);
                var locationName = location != null ? location.Name : "Unknown";


                response.Add(new EventRequestDto
                {
                    Id = eventM.EventId,
                    Name = eventM.Name,
                    PriceRangeEvent = eventM.PricesEvent,
                    Description = eventM.Description,
                    EventDate = eventM.EventDate,
                    EventType = eventM.EventType,
                    ImageEvent = eventM.ImageEvent,
                    StartTime = eventM.StartTime,
                    FinishTime = eventM.FinishTime,
                    LocationId = eventM.LocationId.ToString(),
                    LocationName = locationName

                });
            }
            return Ok(response);
        }

        [HttpGet("GetEventById")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var eventM = await eventRepository.GetAsync(id);
            var locationDM = await locationRepository.GetAllASync();

            if (eventM != null)
            {
                var location = locationDM.FirstOrDefault(m => m.LocationId == eventM.LocationId);
                var locationName = location != null ? location.Name : "Unknown";
                var locationPath = location != null ? location.LocationPath : "Unknown";

                var response = new EventByIdRequestDto
                {
                    Id = eventM.EventId,
                    Name = eventM.Name,
                    Description = eventM.Description,
                    EventDate = eventM.EventDate,
                    EventType = eventM.EventType,
                    ImageEvent = eventM.ImageEvent,
                    StartTime = eventM.StartTime,
                    FinishTime = eventM.FinishTime,
                    LocationId = eventM.LocationId.ToString(),
                    LocationName = locationName,
                    SeatMapPath = locationPath

                };
                return Ok(response);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var eventM = await eventRepository.GetAsync(id);
            var locationDM = await locationRepository.GetAllASync();

            if(eventM != null)
            {
                var location = locationDM.FirstOrDefault(m => m.LocationId == eventM.LocationId);
                var locationName = location != null ? location.Name : "Unknown";


                var response = new EventRequestDto
                {
                    Id = eventM.EventId,
                    Name = eventM.Name,
                    PriceRangeEvent = eventM.Prices,
                    Description = eventM.Description,
                    EventDate = eventM.EventDate,
                    EventType = eventM.EventType,
                    ImageEvent = eventM.ImageEvent,
                    StartTime = eventM.StartTime,
                    FinishTime = eventM.FinishTime,
                    LocationId = eventM.LocationId.ToString(),
                    LocationName = locationName

                };
                return Ok(response);
            }
            return NotFound();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, EditEventRequestDto editEventRequest)
        {
            if(editEventRequest == null)
            {
                return BadRequest("Invalid model data.");
            }

            var eventM = new Event
            {
                EventId = id,
                Name = editEventRequest.Name,
                Prices = editEventRequest.Prices,
                Description = editEventRequest.Description,
                EventDate = editEventRequest.EventDate,
                EventType = editEventRequest.EventType,
                ImageEvent = editEventRequest.ImageEvent,
                StartTime = editEventRequest.StartTime,
                FinishTime = editEventRequest.FinishTime
            };

            var selectedLocationIdGuid = Guid.Parse(editEventRequest.SelectedLocation);
            var existingLocation = await locationRepository.GetASync(selectedLocationIdGuid);

            if (existingLocation != null)
            {
                eventM.LocationId = existingLocation.LocationId;
            }

            var updatedEvent = await eventRepository.UpdateAsync(eventM);

            if(updatedEvent == null)
            {
                return NotFound();
            }

            var response = new EventRequestDto
            {
                Id = id,
                Name =editEventRequest.Name,
                PriceRangeEvent = editEventRequest.Prices,
                Description = editEventRequest.Description,
                EventType = editEventRequest.EventType,
                ImageEvent = editEventRequest.ImageEvent,
                StartTime=editEventRequest.StartTime,
                FinishTime = editEventRequest.FinishTime,
                LocationId = editEventRequest.SelectedLocation,
                LocationName = existingLocation.Name
            };
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var eventM = await eventRepository.DeleteAsync(id);


                if (eventM == null)
                {
                    return NotFound();
                }

                var response = new EventRequestDto
                {
                    Id = eventM.EventId,
                    Name = eventM.Name,
                    PriceRangeEvent = eventM.Prices,
                    Description = eventM.Description,
                    EventDate = eventM.EventDate,
                    EventType = eventM.EventType,
                    ImageEvent = eventM.ImageEvent,
                    StartTime = eventM.StartTime,
                    FinishTime = eventM.FinishTime,
                    LocationId = eventM.LocationId.ToString()
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
