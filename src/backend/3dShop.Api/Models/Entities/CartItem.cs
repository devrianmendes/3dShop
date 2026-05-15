namespace _3dShop.Api.Models.Entities
{
    public class CartItem : BaseEntity
    {
        public required string ProductNameSnapshot { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }

        //Relations
        public required Cart Cart { get; set; }
        public required Guid CartId { get; set; }
        public required Product Product { get; set; }
        public required Guid ProductId { get; set; }
    }
}
