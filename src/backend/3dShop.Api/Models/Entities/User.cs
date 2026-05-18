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

        //Releations
        public ICollection<Order>? OrderList { get; set; }
        public ICollection<OrderStatusHistory>? OrderStatusHistory { get; set; }
        public Cart? Cart { get; set; }
    }
}
