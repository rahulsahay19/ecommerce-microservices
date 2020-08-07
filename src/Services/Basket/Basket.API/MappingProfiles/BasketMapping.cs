namespace Basket.API.MappingProfiles
{
    using AutoMapper;
    using Entities;
    using EventBusRabbitMQ.Events;
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<Checkout, CheckoutEvent>().ReverseMap();
        }
    }
}
