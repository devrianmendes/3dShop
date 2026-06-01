using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs.Users;
using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services
{
    public class CreateUserService
    {
        private readonly AppDbContext _context;
        public CreateUserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NewUserResponse> CreateUserAsync(NewUserRequest newUserRequest, UserRole role, CancellationToken cancellationToken)
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
                UserRole = role,
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