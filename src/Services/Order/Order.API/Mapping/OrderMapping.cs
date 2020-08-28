namespace Order.API.Mapping
{
    using AutoMapper;
    using EventBusRabbitMQ.Events;
    using Application.Commands;
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<CheckoutEvent, CheckoutOrderCommand>().ReverseMap();
        }
    }
}
