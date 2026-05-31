namespace _3dShop.Api.Models.DTOs.Users
{
    public class AuthUserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
