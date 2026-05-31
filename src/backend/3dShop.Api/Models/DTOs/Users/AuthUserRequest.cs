namespace _3dShop.Api.Models.DTOs.Users
{
    public class AuthUserRequest : AuthUserInterface
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
