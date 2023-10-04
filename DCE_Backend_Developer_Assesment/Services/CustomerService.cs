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
    }
}
