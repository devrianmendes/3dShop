using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required UserRole UserRole { get; set; }
        public required bool IsActive { get; set; }

        //Relations
        public ICollection<Order>? OrderList { get; set; }  //Navegação - Facilita acesso a entidade relacionada no código
        public ICollection<OrderStatusHistory>? OrderStatusHistory { get; set; }  //Navegação - Facilita acesso a entidade relacionada no código
        public Cart? Cart { get; set; }  //Navegação - Facilita acesso a entidade relacionada no código
        public ICollection<RefreshToken>? RefreshToken { get; set; }  //Navegação - Facilita acesso a entidade relacionada no código

        //public User(string name, string email, string passwordHash, UserRole userRole, bool isActive)
        //{
        //    Name = name;
        //    Email = email;
        //    PasswordHash = passwordHash;
        //    UserRole = userRole;
        //    IsActive = isActive;
        //}
    }
}
