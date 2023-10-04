using DCE_Backend_Developer_Assesment.Models;

namespace DCE_Backend_Developer_Assesment.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
    }
}
