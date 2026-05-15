namespace _3dShop.Api.Models.Entities
{
    public class OrderItem : BaseEntity
    {
        public required string ProductNameSnapshot { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }
        public required decimal ItemTotal { get; set; }

        //Relations
        public required Order Order { get; set; }
        public required Guid OrderId { get; set; }
        public required Product Product{ get; set; }
        public required Guid ProductId { get; set; }
    }
}
