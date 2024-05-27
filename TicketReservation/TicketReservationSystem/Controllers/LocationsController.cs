using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Models.DTO;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationRepository locationRepository;

        public LocationsController(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLocationRequestDto addLocationRequest)
        {
            try
            {
                var location = new Location
                {
                    Name = addLocationRequest.Name,
                    Type = addLocationRequest.Type,
                    Address = addLocationRequest.Address,
                    Description = addLocationRequest.Description,
                    Capacity = addLocationRequest.Capacity,
                    LocationPath = addLocationRequest.LocationPath
                };

                await locationRepository.AddASync(location);

                var response = new AddLocationRequestDto
                {
                    LocationId = addLocationRequest.LocationId,
                    Name = addLocationRequest.Name,
                    Type = addLocationRequest.Type,
                    Address = addLocationRequest.Address,
                    Description = addLocationRequest.Description,
                    Capacity = addLocationRequest.Capacity,
                    LocationPath = addLocationRequest.LocationPath
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var locations = await locationRepository.GetAllASync();

                var response = new List<LocationRequestDto>();
                foreach (var location in locations)
                {
                    response.Add(new LocationRequestDto
                    {
                        LocationId = location.LocationId,
                        Name = location.Name,
                        Type = location.Type,
                        Address = location.Address,
                        Description = location.Description,
                        Capacity = location.Capacity,
                        LocationPath = location.LocationPath
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
                var location = await locationRepository.GetASync(id);

                if (location != null)
                {
                    var response = new LocationRequestDto
                    {
                        LocationId = location.LocationId,
                        Name = location.Name,
                        Type = location.Type,
                        Address = location.Address,
                        Description = location.Description,
                        Capacity = location.Capacity,
                        LocationPath = location.LocationPath
                    };
                    return Ok(response);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, EditLocationRequestDto editLocationRequest)
        {
            try
            {
                if (editLocationRequest == null)
                {
                    return BadRequest("Invalid model data.");
                }

                var location = new Location
                {
                    LocationId = id,
                    Name = editLocationRequest.Name,
                    Type = editLocationRequest.Type,
                    Address = editLocationRequest.Address,
                    Description = editLocationRequest.Description,
                    Capacity = editLocationRequest.Capacity,
                    LocationPath = editLocationRequest.LocationPath
                };

                var updatedLocation = await locationRepository.UpdateASync(location);

                if (updatedLocation == null)
                {
                    return NotFound();
                }

                var response = new LocationRequestDto
                {
                    LocationId = location.LocationId,
                    Name = location.Name,
                    Type = location.Type,
                    Address = location.Address,
                    Description = location.Description,
                    Capacity = location.Capacity,
                    LocationPath = location.LocationPath
                };

                return Ok(response);
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
                var deletedLocation = await locationRepository.DeleteASync(id);

                if (deletedLocation == null)
                {
                    return NotFound();
                }

                var response = new LocationRequestDto
                {
                    LocationId = deletedLocation.LocationId,
                    Name = deletedLocation.Name,
                    Type = deletedLocation.Type,
                    Address = deletedLocation.Address,
                    Description = deletedLocation.Description,
                    Capacity = deletedLocation.Capacity,
                    LocationPath = deletedLocation.LocationPath
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
