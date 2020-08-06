namespace Basket.API.Controllers
{
    using System;
    using AutoMapper;
    using EventBusRabbitMQ.Common;
    using EventBusRabbitMQ.Events;
    using EventBusRabbitMQ.Producer;
    using Microsoft.Extensions.Logging;
    using Repositories;
    using Entities;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using static Infrastructure.WebConstants;

    public class BasketController : ApiController
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBusRabbitMqProducer;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepo basketRepo, IMapper mapper, EventBusRabbitMQProducer eventBusRabbitMqProducer, ILogger<BasketController> logger)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
            _eventBusRabbitMqProducer = eventBusRabbitMqProducer;
            _logger = logger;
        }

        [HttpGet(UserName)]
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

        [HttpDelete(UserName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            return Ok(await _basketRepo.DeleteBasket(userName));
        }

        [Route(CheckoutAction)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] Checkout checkout)
        {
            // calculate total price of the basket
            // send the basket to rabbit mq for ordering service to process it
            // remove the basket
            var basket = await _basketRepo.GetBasket(checkout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await _basketRepo.DeleteBasket(basket.UserName);
            if (!basketRemoved)
            {
                return BadRequest();
            }

            var eventBusMsg = _mapper.Map<CheckoutEvent>(checkout);
            eventBusMsg.RequestId = Guid.NewGuid();
            eventBusMsg.TotalPrice = checkout.TotalPrice;
            try
            {
                _eventBusRabbitMqProducer.PublishCheckout(EventBusConstants.CheckoutQueue, eventBusMsg);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occured while sending message to RabbitMQ for checkout: {ex.Message}");
            }

            return Accepted();
        }

    }
}
