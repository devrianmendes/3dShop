using _3dShop.Api.Models.Enums;
using _3dShop.Api.Models.Interfaces;

namespace _3dShop.Api.Models.DTOs
{
    public class AuthUserRequest : IValidateUser
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class NewUserRequest : IValidateUser
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole UserRole { get; set; }
        public required bool IsActive { get; set; }
    }

    public class AuthUserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }

    public class NewUserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
