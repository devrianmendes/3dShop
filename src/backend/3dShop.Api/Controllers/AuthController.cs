using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        //private readonly JwtHelper _jwtHelper;
        private readonly AuthService _authService;
        private readonly IValidator<UserRequestBase> _validator; //Interface fornecida pelo fluent validation

        public AuthController(ILogger<AuthController> logger, /*JwtHelper jwtHelper,*/ IValidator<UserRequestBase> validator, AuthService authService)
        {
            _logger = logger;
            //_jwtHelper = jwtHelper;
            _validator = validator;
            _authService = authService;
        }

        [HttpPost("signin")]
        [ProducesResponseType<AuthUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthUserResponse>> SignInAsync(AuthUserRequest user, Guid deviceId, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(user);

            var token = await _authService.SignInAsync(user, deviceId, cancellationToken);

            Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions //Devolve o RefreshToken por cookie para evitar acesso via JS
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.RefreshTokenExpiration
            });

            return Ok(token.AuthUserResponse); //Devolve somente ID, Nome, Email e AccessToken via body
        }

        /// <summary>
        /// Controller para criação de novos usuários. Permite que usuários não logados criem suas próprias contas.
        /// </summary>
        /// <param name="createUserRequest"></param>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignUpAsync(CreateUserRequest createUserRequest, Guid deviceId, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(createUserRequest);

            CreateUserResponse createdUser = await _authService.SignUpAsync(createUserRequest, deviceId, cancellationToken);

            return Ok(createdUser);
        }

        [HttpPost("refresh")]
        [ProducesResponseType<CreateUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> RefreshTokenAsync(Guid deviceId, CancellationToken cancellationToken)
        {

            var rawRefreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(rawRefreshToken))
            {
                return Unauthorized();
            }

            RefreshTokenResponse refreshedToken = await _authService.RefreshTokenAsync(rawRefreshToken, deviceId, cancellationToken);

            Response.Cookies.Append("refreshToken", refreshedToken.newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshedToken.ExpirationDate
            });

            return Ok(refreshedToken.newAccessToken);
        }
    }
}
