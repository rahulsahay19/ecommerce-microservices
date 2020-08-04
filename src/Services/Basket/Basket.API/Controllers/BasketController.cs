namespace Basket.API.Controllers
{
    using Repositories;
    using Entities;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using static Infrastructure.WebConstants;

    public class BasketController : ApiController
    {
        private readonly IBasketRepo _basketRepo;

        public BasketController(IBasketRepo basketRepo)
        {
            _basketRepo = basketRepo;
        }

        [HttpGet(UseName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Basket>> GetBasket(string userName)
        {
            var basket = await _basketRepo.GetBasket(userName);
            return Ok(basket ?? new Basket(userName));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Basket>> CreateOrUpdateBasket([FromBody]Basket basket)
        {
            return Ok(await _basketRepo.CreateOrUpdateBasket(basket));
        }

        [HttpDelete(UseName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            return Ok(await _basketRepo.DeleteBasket(userName));
        }

    }
}
