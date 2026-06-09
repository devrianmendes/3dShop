using _3dShop.Api.Models.DTOs.Users;
using _3dShop.Api.Models.Enums;
using _3dShop.Api.Models.Interfaces;
using _3dShop.Api.Services;
using _3dShop.Api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<IValidateUser> _validate;
        private readonly UserService _userService;
        public UserController(IValidator<IValidateUser> validate, UserService userService)
        {
            _validate = validate;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")] //Permite que apenas admin criem conta para outros admin/sellers
        [HttpPost("register")]
        [ProducesResponseType<NewUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NewUserResponse>> CreateEmployeeAsync(NewUserRequest newUserRequest, CancellationToken cancellationToken)
        {
            _validate.ValidateAndThrow(newUserRequest);

            NewUserResponse createdUser = await _userService.CreateEmployeeAsync(newUserRequest, cancellationToken);

            return Ok(createdUser);
        }
    }
}