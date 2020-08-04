namespace Catalog.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using static Infrastructure.WebConstants;
    public class CatalogController : ApiController
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger _logger;

        public CatalogController(IProductRepo productRepo, ILogger<CatalogController> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepo.GetProducts();
            return Ok(products);
        }

        [HttpGet(Id, Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepo.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Route(CategoryName)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _productRepo.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepo.Create(product);

            return CreatedAtRoute("GetProduct", new { version="1", id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepo.Update(product));
        }

        [Route(Id)]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
           return Ok(await _productRepo.Delete(id));
        }

    }
}
