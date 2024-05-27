using Microsoft.EntityFrameworkCore;
using TicketReservationSystem.Data;
using TicketReservationSystem.Models.Domain;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            await applicationDbContext.Customers.AddAsync(customer);
            await applicationDbContext.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer?> DeleteAsync(Guid id)
        {
            var existingCustomer = await applicationDbContext.Customers.FindAsync(id);

            if(existingCustomer != null)
            {
                applicationDbContext.Customers.Remove(existingCustomer);
                await applicationDbContext.SaveChangesAsync();

                return existingCustomer;
            }
            return null;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
           return await applicationDbContext.Customers.ToListAsync();
        }

        public Task<Customer?> GetAsync(Guid id)
        {
            return applicationDbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
        }

        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            var existingLocation = await applicationDbContext.Customers.FirstOrDefaultAsync();

            if (existingLocation != null)
            {
                existingLocation.FirstName = customer.FirstName;
                existingLocation.LastName = customer.LastName;
                existingLocation.Email = customer.Email;
                existingLocation.Address = customer.Address;
                existingLocation.Phone = customer.Phone;

                await applicationDbContext.SaveChangesAsync();

                return existingLocation;
            }
            return null;
          
        }
    }
}
