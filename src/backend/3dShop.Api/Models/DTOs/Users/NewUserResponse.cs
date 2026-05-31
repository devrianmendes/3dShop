using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.DTOs.Users
{
    public class NewUserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
