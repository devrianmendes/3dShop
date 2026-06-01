using _3dShop.Api.Models.Interfaces;

namespace _3dShop.Api.Models.DTOs.Users
{
    public class AuthUserRequest : ValidateUserInterface
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
