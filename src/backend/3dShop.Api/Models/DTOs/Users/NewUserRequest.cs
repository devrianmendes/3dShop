using _3dShop.Api.Models.Enums;
using _3dShop.Api.Models.Interfaces;

namespace _3dShop.Api.Models.DTOs.Users
{
    public class NewUserRequest : ValidateUserInterface
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole UserRole { get; set; }
        public required bool IsActive { get; set; }
    }
}
