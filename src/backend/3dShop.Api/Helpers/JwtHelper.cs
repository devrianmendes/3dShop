using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace _3dShop.Api.Helpers
{
    public class JwtHelper
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtHelper(IConfiguration configuration)
        {
            // Pega as configs do appsettings.json
            _secret = configuration["JwtSettings:Secret"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _expirationMinutes = int.Parse(configuration["JwtSettings:ExpirationMinutes"]);
        }

        // Gera token JWT
        //public string GenerateToken(string userId, string email, string name, string role = null)
        public void GenerateToken()

        {
            Console.WriteLine(_secret);
            Console.WriteLine(_issuer);
            Console.WriteLine(_audience);
            Console.WriteLine(_expirationMinutes);
            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, userId),
            //    new Claim(JwtRegisteredClaimNames.Email, email),
            //    new Claim(JwtRegisteredClaimNames.Name, name),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    issuer: _issuer,
            //    audience: _audience,
            //    claims: claims,
            //    expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            //    signingCredentials: creds
            //);

            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Valida token e retorna ClaimsPrincipal
        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // evita tolerância de alguns minutos
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null; // token inválido ou expirado
            }
        }
    }
}
