namespace Order.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

    }
}
