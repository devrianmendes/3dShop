namespace _3dShop.Api.Models.Interfaces
{
    public interface IValidateUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
