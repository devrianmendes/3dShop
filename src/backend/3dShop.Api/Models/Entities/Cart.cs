namespace _3dShop.Api.Models.Entities
{
    public class Cart : BaseEntity
    {
        //Relations
        public ICollection<CartItem>? CartItemList { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public required User Customer { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public required Guid CustomerId { get; set; } //FK
    }
}
