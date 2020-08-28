namespace Order.Application.Mapper
{
    using AutoMapper;
    using Responses;
    using Core.Entities;
    using Commands;
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        }
    }
}
