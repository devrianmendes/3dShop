using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwthelper;

        public AuthService(AppDbContext context, JwtHelper jwthelper)
        {
            _context = context;
            _jwthelper = jwthelper;
        }
        public async Task<AuthUserResponse> SignInAsync(AuthUserRequest user, CancellationToken cancellationToken)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email, cancellationToken);

            if(userExist is null)
            {
                throw new BadRequestException("Usuário ou senha inválidos.");
            }

            bool matchPass = BCrypt.Net.BCrypt.Verify(user.Password, userExist.PasswordHash);

            if(!matchPass)
            {
                throw new BadRequestException("Usuário ou senha inválidos.");
            }

            var token = _jwthelper.GenerateToken(userExist.Id.ToString(), userExist.Email, userExist.Name);

            return new AuthUserResponse()
            {
                Id = userExist.Id,
                Name = userExist.Name,
                Email = userExist.Email,
                Token = token
            };
        }
    }
}
