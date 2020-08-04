namespace Basket.API.Repositories
{
    using Data;
    using Entities;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    public class BasketRepo : IBasketRepo
    {
        private readonly IBasketContext _basketContext;

        public BasketRepo(IBasketContext basketContext)
        {
            _basketContext = basketContext;
        }
        public async Task<Basket> GetBasket(string userName)
        {
            var basket = await _basketContext.Redis.StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            // Deserialize and sending
            return JsonConvert.DeserializeObject<Basket>(basket);
        }

        public async Task<Basket> CreateOrUpdateBasket(Basket basket)
        {
            var updated = await _basketContext.Redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            if (!updated)
            {
                return null;
            }

            return await GetBasket(basket.UserName);
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            return await _basketContext.Redis.KeyDeleteAsync(userName);
        }
    }
}
