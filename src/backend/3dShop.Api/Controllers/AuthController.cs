using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services;
using _3dShop.Api.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JwtHelper _jwtHelper;
        private readonly AuthService _authService;
        private readonly IValidator<AuthUserRequest> _validator; //Interface fornecida pelo fluent validation

        public AuthController(ILogger<AuthController> logger, JwtHelper jwtHelper, IValidator<AuthUserRequest> validator, AuthService authService)
        {
            _logger = logger;
            _jwtHelper = jwtHelper;
            _validator = validator;
            _authService = authService; 
        }

        [HttpPost]
        [ProducesResponseType<AuthUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthUserResponse>> SignInAsync(AuthUserRequest user, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(user);

            AuthUserResponse token = await _authService.SignInAsync(user, cancellationToken);

            return Ok(token);
        }

        //[HttpGet("test")]
        //public async Task<IActionResult> Test()
        //{


        //    var token = Request.Headers["Authorization"];

        //    Console.WriteLine(token);

        //    return Ok(User.Identity.IsAuthenticated);
        //}

        [Authorize]
        [HttpGet]
        public ActionResult Test()
        {
            return Ok(User.Identity.IsAuthenticated);
        }
    }
}
