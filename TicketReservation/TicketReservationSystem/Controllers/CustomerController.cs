using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Models.DTO;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerRequestDto addCustomerRequest)
        {
            try
            {
                if (addCustomerRequest == null)
                {
                    return BadRequest("Invalid model data.");
                };

                var customer = new Customer
                {
                    FirstName = addCustomerRequest.FirstName,
                    LastName = addCustomerRequest.LastName,
                    Email = addCustomerRequest.Email,
                    Phone = addCustomerRequest.Phone,
                    Address = addCustomerRequest.Address,
                };

                await customerRepository.AddAsync(customer);

                var response = new AddCustomerRequestDto
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var customers = await customerRepository.GetAllAsync();

                var response = new List<AddCustomerRequestDto>();
                foreach (var customer in customers)
                {
                    response.Add(new AddCustomerRequestDto
                    {
                        CustomerId = customer.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.Phone,
                        Address = customer.Address,
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var customer = await customerRepository.DeleteAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                var response = new AddCustomerRequestDto
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
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
