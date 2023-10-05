using DCE_Backend_Developer_Assesment.DTO.Requests;
using DCE_Backend_Developer_Assesment.Models;
using DCE_Backend_Developer_Assesment.Repositories;
using System;
using System.Collections.Generic;

namespace DCE_Backend_Developer_Assesment.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Retrieves a list of all customers.
        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        // Deletes a customer by their ID.
        public bool DeleteCustomer(Guid id)
        {
            return _customerRepository.DeleteCustomer(id);
        }

        // Updates customer information.
        public bool UpdateCustomer(Guid id, string? username, string? email, string? firstName, string? lastName)
        {
            return _customerRepository.UpdateCustomer(id, username, email, firstName, lastName);
        }

        // Registers a new customer.

        // Registers a new customer.
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

        // Retrieves a customer by their ID.
        public Customer GetCustomerById(Guid id)
        {
            // Call the repository to get the customer by ID
            return _customerRepository.GetCustomerById(id);
        }
    }
}
