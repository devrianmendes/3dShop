using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public string GenerateToken(string userId, string email, string name, string role)
        {
            var claims = new[]
            {
                //Claim é um tipo do asp.net utilizado para identificação. Armazenamos cada dado do usuario em um claim e os claims agrupados em um array. Formato do claim: {chave: valor}
                //JwtRegisteredClaimNames é um objeto com chaves como as abaixo. Serve apenas para não precisar escrever o nome do campo com string ("sub", "email"...). Simples assim.
                new Claim(JwtRegisteredClaimNames.Sub, userId), 
                new Claim(JwtRegisteredClaimNames.Email, email), 
                new Claim(JwtRegisteredClaimNames.Name, name), 
                new Claim(ClaimTypes.Role, role),
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

        // Valida token e retorna ClaimsPrincipal
        //public ClaimsPrincipal? ValidateToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler(); //Validador do JWT
        //    var key = Encoding.UTF8.GetBytes(_secret); //Novamente converte o secret em byte

        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = _issuer,
        //            ValidAudience = _audience,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ClockSkew = TimeSpan.Zero // evita tolerância de alguns minutos
        //        }, out SecurityToken validatedToken);

        //        return principal;
        //    }
        //    catch
        //    {
        //        return null; // token inválido ou expirado
        //    }
        //}
    }
}
