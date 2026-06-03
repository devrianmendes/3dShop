using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs.Users;
using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Interfaces;
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
        private readonly IValidator<ValidateUserInterface> _validator; //Interface fornecida pelo fluent validation

        public AuthController(ILogger<AuthController> logger, JwtHelper jwtHelper, IValidator<ValidateUserInterface> validator, AuthService authService)
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
        [ProducesResponseType<NewUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignUpAsync(NewUserRequest newUserRequest, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(newUserRequest);

            NewUserResponse createdUser = await _authService.SignUpAsync(newUserRequest, cancellationToken);

            return Ok(createdUser);
        }

        [HttpPost("refresh")]
        [ProducesResponseType<NewUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RefreshTokenAsync(NewUserRequest newUserRequest, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(newUserRequest);

            NewUserResponse createdUser = await _authService.SignUpAsync(newUserRequest, cancellationToken);

            return Ok(createdUser);
        }
    }
}
