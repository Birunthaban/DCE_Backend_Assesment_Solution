
using DCE_Backend_Developer_Assesment.Repositories;
using DCE_Backend_Developer_Assesment.DTO.Responses;

namespace DCE_Backend_Developer_Assesment.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;


        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<ActiveOrderResponse> GetActiveOrdersByCustomer(Guid customerId)
        {
            return _orderRepository.GetActiveOrdersByCustomer(customerId);
        }

    }
}
