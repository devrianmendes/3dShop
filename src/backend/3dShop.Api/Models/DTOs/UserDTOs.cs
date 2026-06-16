using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.DTOs
{
    // --- Bases ---

    public abstract record UserRequestBase
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public abstract record UserResponseBase
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
    }

    //--- Requests ---

    public record AuthUserRequest : UserRequestBase;

    public record CreateUserRequest : UserRequestBase
    {
        public required string Name { get; set; }
        public required UserRole UserRole { get; set; }
        public required bool IsActive { get; set; }
        //public Guid? DeviceId { get; set; }
    }

    //--- Responses ---

    public record AuthUserResponse : UserResponseBase
        {
            public required string AccessToken { get; set; }
            //public required RefreshToken RefreshToken { get; set; }
    }

    public record CreateUserResponse : UserResponseBase;



}
