namespace Order.Application.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Commands;
    using Responses;
    using Core.Repositories;
    using Core.Entities;
    using Mapper;
    using System;
    // If it gets request from CheckoutOrderCommand, then return OrderResponse and run the below handle class 
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand,OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = OrderMapper.Mapper.Map<Order>(request);
            if (orderEntity == null)
            {
                throw new ApplicationException("Mapping Issue");
            }

            var newOrder = await _orderRepository.AddAsync(orderEntity);
            var orderResponse = OrderMapper.Mapper.Map<OrderResponse>(newOrder);
            return orderResponse;
        }
    }
}
