using DCE_Backend_Developer_Assesment.Services;
using Microsoft.AspNetCore.Mvc;

namespace DCE_Backend_Developer_Assesment.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController :ControllerBase
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

    }
}
