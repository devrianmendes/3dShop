using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IValidator<UserRequestBase> _validator; //Interface fornecida pelo fluent validation

        public AuthController(ILogger<AuthController> logger, JwtHelper jwtHelper, IValidator<UserRequestBase> validator, AuthService authService)
        {
            _logger = logger;
            _jwtHelper = jwtHelper;
            _validator = validator;
            _authService = authService;
        }

        [HttpPost("signin")]
        [ProducesResponseType<AuthUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthUserResponse>> SignInAsync(AuthUserRequest user, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(user);

            AuthUserResponse token = await _authService.SignInAsync(user, cancellationToken);

            return Ok(token);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("register")]
        [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignUpAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(createUserRequest);

            CreateUserResponse createdUser = await _authService.SignUpAsync(createUserRequest, cancellationToken);

            return Ok(createdUser);
        }

        [HttpPost("refresh")]
        [ProducesResponseType<CreateUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RefreshTokenAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(createUserRequest);

            CreateUserResponse createdUser = await _authService.SignUpAsync(createUserRequest, cancellationToken);

            return Ok(createdUser);
        }
    }
}
