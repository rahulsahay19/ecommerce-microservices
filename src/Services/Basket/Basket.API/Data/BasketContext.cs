namespace Basket.API.Data
{
    using StackExchange.Redis;
    public class BasketContext: IBasketContext
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public BasketContext(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            Redis = _connectionMultiplexer.GetDatabase();
        }
        public IDatabase Redis { get; }
    }
}
