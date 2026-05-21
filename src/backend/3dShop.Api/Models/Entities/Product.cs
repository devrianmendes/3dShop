namespace _3dShop.Api.Models.Entities
{
    public class Product : BaseEntity
    {
        public required string NamePt { get; set; }
        public required string NameEn { get; set; }
        public required string DescriptionPt { get; set; }
        public required string DescriptionEn { get; set; }
        public required decimal Price { get; set; }
        public required bool IsCustom { get; set; }
        public required bool IsActive { get; set; }
        
        //Relations
        public required Guid CategoryId { get; set; } //FK
        public required Category Category { get; set; } //Navegação - Facilita acesso a entidade relacionada no código

        public ICollection<ProductImage>? ProductImageList { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public ICollection<OrderItem>? OrderItemList { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public ICollection<CartItem>? CartItemList { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
    }
}
