using DCE_Backend_Developer_Assesment.DTO.Responses;

namespace DCE_Backend_Developer_Assesment.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<ActiveOrderResponse> GetActiveOrdersByCustomer(Guid customerId);
    }
}
