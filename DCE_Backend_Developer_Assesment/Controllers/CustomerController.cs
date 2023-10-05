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

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();

            if (customers == null || !customers.Any())
            {
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return BadRequest(problemDetails);
            }

            return Ok(customers);
        }

        [HttpPost("register")]
        public IActionResult RegisterCustomer([FromBody] CustomerRegistrationRequest registrationRequest)
        {
            if (!ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return BadRequest(problemDetails);
            }

            Customer createdCustomer = _customerService.RegisterCustomer(registrationRequest);

            if (createdCustomer != null)
            {
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.UserId }, createdCustomer);
            }
            else
            {
                ModelState.AddModelError("RegistrationError", "Registration failed");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return BadRequest(problemDetails);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            bool deleted = _customerService.DeleteCustomer(id);

            if (deleted)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("CustomerNotFound", "Customer not found");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public IActionResult GetCustomerById(Guid id)
        {
            Customer customer = _customerService.GetCustomerById(id);

            if (customer == null)
            {
                ModelState.AddModelError("CustomerNotFound", "Customer not found");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return BadRequest(problemDetails);
            }

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateRequest updateRequest)
        {
            if (id != updateRequest.UserId)
            {
                ModelState.AddModelError("CustomerIdMismatch", "Customer ID mismatch");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return BadRequest(problemDetails);
            }

            bool updated = _customerService.UpdateCustomer(id, updateRequest.Username, updateRequest.Email, updateRequest.FirstName, updateRequest.LastName);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("UpdateError", "Email is already in use or no fields to update");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("{customerId}/active-orders")]
        public IActionResult GetActiveOrdersByCustomer(Guid customerId)
        {
            var activeOrders = _orderService.GetActiveOrdersByCustomer(customerId);

            if (activeOrders == null || !activeOrders.Any())
            {
                ModelState.AddModelError("NoActiveOrders", "No active orders found");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return BadRequest(problemDetails);
            }

            return Ok(activeOrders);
        }
    }
}
