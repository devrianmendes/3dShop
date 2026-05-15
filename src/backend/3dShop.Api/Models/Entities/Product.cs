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
        public required Guid CategoryId { get; set; }
        public required Category Category { get; set; }

        public ICollection<ProductImage>? ProductImageList { get; set; }
        public ICollection<OrderItem>? OrderItemList { get; set; }
        public ICollection<CartItem>? CartItemList { get; set; }
    }
}
