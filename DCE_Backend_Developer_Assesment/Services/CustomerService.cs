using DCE_Backend_Developer_Assesment.DTO.Requests;
using DCE_Backend_Developer_Assesment.Models;
using DCE_Backend_Developer_Assesment.Repositories;

namespace DCE_Backend_Developer_Assesment.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }
 

            public bool DeleteCustomer(Guid id)
            {
                return _customerRepository.DeleteCustomer(id);
            }


        public bool UpdateCustomer(Guid id, string? username, string? email, string? firstName, string? lastName)
        {
            return _customerRepository.UpdateCustomer(id, username, email, firstName, lastName);
        }


        public Customer RegisterCustomer(CustomerRegistrationRequest registrationRequest)
        {
            // Check if the email is already in use
            if (_customerRepository.IsEmailInUse(registrationRequest.Email))
            {
                return null; // Return null to indicate registration failure (duplicate email)
            }

            // Create a new Customer instance without specifying UserId
            Customer newCustomer = new Customer
            {
                Username = registrationRequest.Username,
                Email = registrationRequest.Email,
                FirstName = registrationRequest.FirstName,
                LastName = registrationRequest.LastName,
                IsActive = true // You can set IsActive to true by default for new registrations
            };

            // Call the repository to add the new customer to the database
            return _customerRepository.AddCustomer(newCustomer);
        }

        public Customer GetCustomerById(Guid id)
        {
            // Call the repository to get the customer by ID
            return _customerRepository.GetCustomerById(id);
        }
    }
}
