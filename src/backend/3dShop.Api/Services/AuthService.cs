using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Helpers;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Entities;
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

        public record SignInResult
        {
            public required AuthUserResponse AuthUserResponse { get; init; }
            public required string RefreshToken { get; init; }
            public DateTime RefreshTokenExpiration { get; init; }
        }

        public record RefreshTokenResult
        {
            public required string newAccessToken { get; init; }
            public required string newRefreshToken { get; init; }
        }

        public async Task<SignInResult> SignInAsync(AuthUserRequest user, CancellationToken cancellationToken)
        {
            var userExist = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == user.Email, cancellationToken);

            var hash = userExist?.PasswordHash ?? "$2a$11$dummyhashvaluetomakeconstanttime"; //Verifica se o password existe, se não houver define um dummy
            bool matchPass = BCrypt.Net.BCrypt.Verify(user.Password, hash); //Verifica se o password enviad peloo usuárioo bate com o registrad no banco;

            if (userExist is null || !matchPass) //Verifica a validade dos dois ao mesmo tempo para evitar deduções de ataques por tempo de resposta
                throw new UnauthorizedException("Usuário ou senha inválidos.");

            string accessToken = _jwthelper.GenerateAccessToken(userExist.Id, userExist.Email, userExist.Name, userExist.UserRole);
            RefreshToken refreshToken = _jwthelper.GenerateRefreshToken(userExist.Id);

            await _context.RefreshToken.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new SignInResult()
            {
                AuthUserResponse = new AuthUserResponse()
                {
                    Id = userExist.Id,
                    Email = userExist.Email,
                    Name = userExist.Name,
                    AccessToken = accessToken
                },
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpirationDate
            };
        }

        //Service externalizado para prevenir duplicação de código, pois criar um admin/seller e um usuário normal é exatamente o mesmo código, com exceção da role
        public async Task<CreateUserResponse> SignUpAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            return await _service.CreateUserAsync(createUserRequest, UserRole.Customer, cancellationToken);
        }

        public async Task<RefreshTokenResult> RefreshTokenAsync(RefreshTokenRequest actualToken, CancellationToken cancellationToken)
        {
            var tokenExist = await _context.RefreshToken.FirstOrDefaultAsync(t => t.Token == actualToken.RefreshToken, cancellationToken);

            if(tokenExist is null)
            {
                throw new UnauthorizedException("Refresh Token não existe.");
            }

            if (tokenExist.ExpirationDate <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Token inválido.");
            }

            if (tokenExist.RevokedAt is not null)
            {
                throw new UnauthorizedException("Token inválido.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == tokenExist.UserId, cancellationToken);

            if(user is null)
            {
                throw new BadRequestException("Token inválido");
            }

            var newAccessToken = _jwthelper.GenerateAccessToken(user.Id, user.Email, user.Name, user.UserRole);
            var newRefreshToken = _jwthelper.GenerateRefreshToken(user.Id, actualToken);

            await _context.RefreshToken.AddAsync(newRefreshToken, cancellationToken);

            RefreshTokenResult newToken = new()
            {
                newAccessToken = newAccessToken,
                newRefreshToken = newRefreshToken.Token,
            };

            return newToken;
        }
    }
}