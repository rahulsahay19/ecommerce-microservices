namespace Basket.API.Data
{
    using StackExchange.Redis;
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}
