using DCE_Backend_Developer_Assesment.DTO.Responses;

namespace DCE_Backend_Developer_Assesment.Services
{
    public interface IOrderService
    {
        IEnumerable<ActiveOrderResponse> GetActiveOrdersByCustomer(Guid customerId);
    }
}
