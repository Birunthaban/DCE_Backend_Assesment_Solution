using DCE_Backend_Developer_Assesment.Models;

namespace DCE_Backend_Developer_Assesment.Repositories
{
    public interface ICustomerRepository
    {
        Customer AddCustomer(Customer newCustomer);
        IEnumerable<Customer> GetAll();
        bool IsEmailInUse(string email);
        Customer GetCustomerById(Guid id);
        bool DeleteCustomer(Guid id);
        bool UpdateCustomer(Guid id, string? username, string? email, string? firstName, string? lastName);
    }
}
