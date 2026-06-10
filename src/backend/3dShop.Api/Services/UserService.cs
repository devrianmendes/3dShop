using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services.Internals;

namespace _3dShop.Api.Services
{
    public class UserService
    {
        private readonly CreateUserService _createUserService;
        public UserService(CreateUserService createUserService)
        {
            _createUserService = createUserService;
        }

        //Service externalizado para prevenir duplicação de código, pois criar um admin/seller e um usuário normal é exatamente o mesmo código, com exceção da role
        public async Task<CreateUserResponse> CreateEmployeeAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            return await _createUserService.CreateUserAsync(createUserRequest, createUserRequest.UserRole, cancellationToken);
        }
    }

}