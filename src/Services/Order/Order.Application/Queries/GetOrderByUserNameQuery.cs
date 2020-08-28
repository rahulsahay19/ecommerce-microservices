namespace Order.Application.Queries
{
    using System.Collections.Generic;
    using MediatR;
    using Responses;
    public class GetOrderByUserNameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; set; }
        public GetOrderByUserNameQuery(string userName)
        {
            UserName = userName;
        }
        
    }
}
