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

        /// <summary>
        /// Service para criação de novos usuários (apenas empregados, criados por um admin)
        /// </summary>
        /// <param name="createUserRequest">Dados do usuário.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns></returns>
        //
        public async Task<CreateUserResponse> CreateEmployeeAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            return await _createUserService.CreateUserAsync(createUserRequest, createUserRequest.UserRole, cancellationToken);
        }
    }

}