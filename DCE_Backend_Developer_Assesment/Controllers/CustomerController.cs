using DCE_Backend_Developer_Assesment.DTO.Requests;
using DCE_Backend_Developer_Assesment.Models;
using DCE_Backend_Developer_Assesment.Services;
using Microsoft.AspNetCore.Mvc;

namespace DCE_Backend_Developer_Assesment.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();

            // Check if the customers collection is empty or null
            if (customers == null || !customers.Any())
            {
                return NotFound(); // Return a 404 Not Found response if no customers are found
            }

            return Ok(customers); // Return a 200 OK response with the list of customers
        }
        [HttpPost("register")]
        public IActionResult RegisterCustomer([FromBody] CustomerRegistrationRequest registrationRequest)
        {
            // Validate the incoming registration request data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            // Pass the registration request to the service for registration
            Customer createdCustomer = _customerService.RegisterCustomer(registrationRequest);

            if (createdCustomer != null)
            {
                // Return an HTTP Created (201) response with the created customer object
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.UserId }, createdCustomer);
            }
            else
            {
                // Handle registration failure (e.g., duplicate email)
                // Return an appropriate response
                return BadRequest(new { Message = "Registration failed" });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            bool deleted = _customerService.DeleteCustomer(id);

            if (deleted)
            {
                return NoContent(); // Return a 204 No Content response on successful delete
            }
            else
            {
                return NotFound(); // Return a 404 Not Found response if the customer is not found
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateRequest updateRequest)
        {
            if (id != updateRequest.UserId)
            {
                return BadRequest("Customer ID mismatch."); // Return a 400 Bad Request if the provided ID doesn't match the updated customer's ID
            }

            bool updated = _customerService.UpdateCustomer(id, updateRequest.Username, updateRequest.Email, updateRequest.FirstName, updateRequest.LastName);

            if (updated)
            {
                return NoContent(); // Return a 204 No Content response on successful update
            }
            else
            {
                return BadRequest("Email is already in use or no fields to update."); // Return a 400 Bad Request if the email is already in use or no fields to update
            }
        }



        [HttpGet("{id}", Name = "GetCustomerById")]
        public IActionResult GetCustomerById(Guid id)
        {
            // Implement logic to retrieve a customer by ID using your service/repository
            Customer customer = _customerService.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound(); // Return a 404 Not Found response if the customer is not found
            }

            return Ok(customer); // Return a 200 OK response with the customer object
        }



    }

}

