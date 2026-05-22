using _3dShop.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        private readonly ILogger<TesteController> _logger;
        private readonly JwtHelper _jwtHelper;


        public TesteController(ILogger<TesteController> logger, JwtHelper jwtHelper)
        {
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Rota /test chamada");

            _jwtHelper.GenerateToken("id", "email", "nome", "admin");
            return Ok("OK");
        }
    }
}
