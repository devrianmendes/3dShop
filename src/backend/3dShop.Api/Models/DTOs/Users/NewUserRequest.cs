using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.DTOs.Users
{
    public class NewUserRequest : AuthUserInterface
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole UserRole { get; set; }
        public required bool IsActive { get; set; }
    }
}
