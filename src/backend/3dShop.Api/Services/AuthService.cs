using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        
        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AuthUserResponse> Execute(AuthUserRequest user)
        {
            var userExist = await _context.Users.AnyAsync(u => u.Email == user.Email);

            if(!userExist)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            return new AuthUserResponse();
        }
    }
}
