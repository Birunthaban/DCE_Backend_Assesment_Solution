using DCE_Backend_Developer_Assesment.DTO.Requests;
using DCE_Backend_Developer_Assesment.Models;
using DCE_Backend_Developer_Assesment.Repositories;
using DCE_Backend_Developer_Assesment.Services;
using DCE_Backend_Developer_Assessment.DTO.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DCE_Backend_Developer_Assesment.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public CustomerController(ICustomerService customerService, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        // GET: api/customers
        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();

            // Check if no customers were found
            if (customers == null || !customers.Any())
            {
                // Create a ValidationProblemDetails with a 404 status for not found
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }

            // Return a 200 OK response with the list of customers
            return Ok(customers);
        }

        // POST: api/customers/register
        [HttpPost("register")]
        public IActionResult RegisterCustomer([FromBody] CustomerRegistrationRequest registrationRequest)
        {
            // Check if the request data is valid
            if (!ModelState.IsValid)
            {
                // Create a ValidationProblemDetails with a 400 status for bad request
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }

            // Register the customer using the service
            Customer createdCustomer = _customerService.RegisterCustomer(registrationRequest);

            if (createdCustomer != null)
            {
                // Return a 201 Created response with the created customer object
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.UserId }, createdCustomer);
            }
            else
            {
                // Add an error to ModelState and create a ValidationProblemDetails with a 400 status for bad request
                ModelState.AddModelError("RegistrationError", "Registration failed");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            // Delete the customer using the service
            bool deleted = _customerService.DeleteCustomer(id);

            if (deleted)
            {
                // Return a 204 No Content response on successful delete
                return NoContent();
            }
            else
            {
                // Add an error to ModelState and create a ValidationProblemDetails with a 404 status for not found
                ModelState.AddModelError("CustomerNotFound", "Customer not found");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }
        }

        // GET: api/customers/{id}
        [HttpGet("{id}", Name = "GetCustomerById")]
        public IActionResult GetCustomerById(Guid id)
        {
            // Retrieve the customer by ID using the service
            Customer customer = _customerService.GetCustomerById(id);

            if (customer == null)
            {
                // Add an error to ModelState and create a ValidationProblemDetails with a 404 status for not found
                ModelState.AddModelError("CustomerNotFound", "Customer not found");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }

            // Return a 200 OK response with the customer object
            return Ok(customer);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateRequest updateRequest)
        {
            // Check if the provided ID matches the updated customer's ID
            if (id != updateRequest.UserId)
            {
                // Add an error to ModelState and create a ValidationProblemDetails with a 400 status for bad request
                ModelState.AddModelError("CustomerIdMismatch", "Customer ID mismatch");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }

            // Update the customer using the service
            bool updated = _customerService.UpdateCustomer(id, updateRequest.Username, updateRequest.Email, updateRequest.FirstName, updateRequest.LastName);

            if (updated)
            {
                // Return a 204 No Content response on successful update
                return NoContent();
            }
            else
            {
                // Add an error to ModelState and create a ValidationProblemDetails with a 400 status for bad request
                ModelState.AddModelError("UpdateError", "Email is already in use or no fields to update");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }
        }

        // GET: api/customers/{customerId}/active-orders
        [HttpGet("{customerId}/active-orders")]
        public IActionResult GetActiveOrdersByCustomer(Guid customerId)
        {
            // Retrieve active orders by customer ID using the order service
            var activeOrders = _orderService.GetActiveOrdersByCustomer(customerId);

            if (activeOrders == null || !activeOrders.Any())
            {
                // Add an error to ModelState and create a ValidationProblemDetails with a 404 status for not found
                ModelState.AddModelError("NoActiveOrders", "No active orders found");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                // Return a BadRequest response with the problem details
                return BadRequest(problemDetails);
            }

            // Return a 200 OK response with the list of active orders
            return Ok(activeOrders);
        }
    }
}
