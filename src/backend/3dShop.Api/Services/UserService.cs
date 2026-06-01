using _3dShop.Api.Models.DTOs.Users;

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
        public async Task<NewUserResponse> CreateEmployeeAsync(NewUserRequest newUserRequest, CancellationToken cancellationToken)
        {
            return await _createUserService.CreateUserAsync(newUserRequest, newUserRequest.UserRole, cancellationToken);
        }
    }

}