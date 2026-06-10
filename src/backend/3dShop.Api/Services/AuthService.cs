using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Enums;
using _3dShop.Api.Services.Internals;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services
{
    public class AuthService
    {
        private readonly CreateUserService _service;
        private readonly JwtHelper _jwthelper;
        private readonly AppDbContext _context;

        public AuthService(CreateUserService createUserService, AppDbContext context, JwtHelper jwthelper)
        {
            _service = createUserService;
            _context = context;
            _jwthelper = jwthelper;
        }
        public async Task<AuthUserResponse> SignInAsync(AuthUserRequest user, CancellationToken cancellationToken)
        {
            var userExist = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == user.Email, cancellationToken);

            var hash = userExist?.PasswordHash ?? "$2a$11$dummyhashvaluetomakeconstanttime"; //Verifica se o password existe, se não houver define um dummy
            bool matchPass = BCrypt.Net.BCrypt.Verify(user.Password, hash); //Verifica se o password enviad peloo usuárioo bate com o registrad no banco;

            if (userExist is null || !matchPass) //Verifica a validade dos dois ao mesmo tempo para evitar deduções de ataques por tempo de resposta
                throw new BadRequestException("Usuário ou senha inválidos.");

            var token = _jwthelper.GenerateToken(userExist.Id.ToString(), userExist.Email, userExist.Name, userExist.UserRole.ToString());

            return new AuthUserResponse()
            {
                Id = userExist.Id,
                Name = userExist.Name,
                Email = userExist.Email,
                Token = token
            };
        }

        //Service externalizado para prevenir duplicação de código, pois criar um admin/seller e um usuário normal é exatamente o mesmo código, com exceção da role
        public async Task<CreateUserResponse> SignUpAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            return await _service.CreateUserAsync(createUserRequest, UserRole.Customer, cancellationToken);
        }
    }
}