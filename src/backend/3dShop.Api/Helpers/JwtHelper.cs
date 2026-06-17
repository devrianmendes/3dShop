using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace _3dShop.Api.Helpers
{
    public class JwtHelper
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;
        private readonly IHttpContextAccessor _httpContext;

        public JwtHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            // Pega as configs do appsettings.json
            _secret = configuration["JwtSettings:Secret"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _expirationMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"]);
            _httpContext = httpContextAccessor;
        }

        /// <summary>
        /// Gera um novo AccessToken para um usuário.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="email">E-mail do usuário.</param>
        /// <param name="name">Nome do usuário.</param>
        /// <param name="role">Role do usuário.</param>
        /// <returns>Retorna o token criado.</returns>
        public string GenerateAccessToken(Guid userId, string email, string name, UserRole role)
        {
            var claims = new[]
            {
                //Claim é um tipo do asp.net utilizado para identificação. Armazenamos cada dado do usuario em um claim e os claims agrupados em um array. Formato do claim: {chave: valor}
                //JwtRegisteredClaimNames é um objeto com chaves como as abaixo. Serve apenas para não precisar escrever o nome do campo com string ("sub", "email"...). Simples assim.
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), 
                new Claim(JwtRegisteredClaimNames.Email, email), 
                new Claim(JwtRegisteredClaimNames.Name, name), 
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //Um novo id que ajuda a instanciar um token. Se um usuário logasse em dispositivos diferentes sem esse dado extra, seus tokens seriam exatamente iguais.
            }; //Formato final: [{sub: userId}, {Email: email}, {Name: name}, {Jti: new Guid}]

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)); //converte o _secret de string para bytes e o transforma em uma chave utilizado pelo JWT;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //define como o token será assinado (bytes do secrets + algoritmo de criptografia)

            var token = new JwtSecurityToken(
                issuer: _issuer, //Quem criou o token (3dShop.Api). Pode ser usado para saber se o token que volta pro frontend foi enviado pela api correta.
                audience: _audience, //Para quem o token foi feito (3dShop.Frontend). Pode ser usado para confirmar se o destinatário está correto.
                claims: claims, //Payload do token, contém dados do usuário
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes), //Data de expiração do token.
                signingCredentials: creds //Adiciona secret e algoritmo de criptografia para garantir integridade (não sofreu alterações) do token;
             );

            return new JwtSecurityTokenHandler().WriteToken(token); //Retorna o token transformado em string;
        }

        /// <summary>
        /// Gera um novo RefreshToken para um usuário.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="deviceId">Dispositivo do usuário.</param>
        /// <returns></returns>
        public RefreshToken GenerateRefreshToken(Guid userId, Guid? deviceId)
        {
            return new RefreshToken()
            {
                UserId = userId,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                RevokedAt = null,
                ReplacedByToken = null,
                DeviceId = deviceId.ToString(),
                UserAgent = _httpContext.HttpContext?.Request.Headers.UserAgent.ToString(),
                IpAddress = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            };
        }

        /// <summary>
        /// Revoga um RefreshToken definindo RevoketAt e ReplacedByToken do token existente.
        /// </summary>
        /// <param name="oldToken">Token antigo</param>
        /// <param name="newToken">Token novo (preenche o campo ReplacedByToken)</param>
        public void RevokeRefreshToken(RefreshToken oldToken, string newToken)
        {
            oldToken.RevokedAt = DateTime.UtcNow;
            oldToken.ReplacedByToken = newToken;
        }
    }
}
