using DCE_Backend_Developer_Assesment.Repositories;
using DCE_Backend_Developer_Assesment.DTO.Responses;
using System;
using System.Collections.Generic;

namespace DCE_Backend_Developer_Assesment.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        // Constructor to inject the order repository.
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Retrieves a list of active orders for a customer.
        public IEnumerable<ActiveOrderResponse> GetActiveOrdersByCustomer(Guid customerId)
        {
            return _orderRepository.GetActiveOrdersByCustomer(customerId);
        }
    }
}
