using DCE_Backend_Developer_Assesment.DTO.Requests;
using DCE_Backend_Developer_Assesment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace DCE_Backend_Developer_Assesment.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        Customer RegisterCustomer(CustomerRegistrationRequest registrationRequest);
        Customer GetCustomerById(Guid id);
    }
}
