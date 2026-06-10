using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Interfaces;
using _3dShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<CreateUserRequest> _validate;
        private readonly UserService _userService;
        public UserController(IValidator<CreateUserRequest> validate, UserService userService)
        {
            _validate = validate;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")] //Permite que apenas admin criem conta para outros admin/sellers
        [HttpPost("register")]
        [ProducesResponseType<CreateUserRequest>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserResponse>> CreateEmployeeAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            await _validate.ValidateAndThrowAsync(createUserRequest, cancellationToken);

            CreateUserResponse createdUser = await _userService.CreateEmployeeAsync(createUserRequest, cancellationToken);

            return Ok(createdUser);
        }
    }
}