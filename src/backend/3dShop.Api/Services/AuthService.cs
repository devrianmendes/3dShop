using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs.Users;
using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Enums;
using BCrypt.Net;
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

        public async Task<NewUserResponse> SignUpAsync(NewUserRequest newUserRequest, CancellationToken cancellationToken)
        {
            var userExist = await _context.Users.AnyAsync(u => u.Email == newUserRequest.Email, cancellationToken);

            if (userExist)
            {
                throw new BadRequestException("Email já cadastrado.");
            }

            User newUser = new()
            {
                Name = newUserRequest.Name,
                Email = newUserRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUserRequest.Password),
                IsActive = true,
                UserRole = UserRole.Customer,
            };

            await _context.Users.AddAsync(newUser, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new NewUserResponse()
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
            };
        }
    }
}