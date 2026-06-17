using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services.Internals
{
    public class CreateUserService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;
        public CreateUserService(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Core da criação do usuário. Externalizado para utilização pelos endpoints /auth/register e /user/register
        /// </summary>
        /// <param name="newUserRequest">Dados do usuário.</param>
        /// <param name="role">Role do usuário.</param>
        /// <param name="deviceId">Dispositivo do usuário.</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Retorna Id, Nome, Email e Tokens do usuário.</returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<CreateUserResponse> CreateUserAsync(
            CreateUserRequest newUserRequest, 
            UserRole role, 
            CancellationToken cancellationToken,
            Guid? deviceId = null)
        {
            var userExist = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == newUserRequest.Email, cancellationToken);

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
                UserRole = role,
            };

            var refreshToken = _jwtHelper.GenerateRefreshToken(newUser.Id, deviceId);

            await _context.Users.AddAsync(newUser, cancellationToken);
            await _context.RefreshToken.AddAsync(refreshToken, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateUserResponse()
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
            };
        }
    }

}