namespace Basket.API.Repositories
{
    using System.Threading.Tasks;
    using Entities;
    public interface IBasketRepo
    {
        Task<Basket> GetBasket(string userName);
        Task<Basket> CreateOrUpdateBasket(Basket basket);
        Task<bool> DeleteBasket(string userName);
    }
}
