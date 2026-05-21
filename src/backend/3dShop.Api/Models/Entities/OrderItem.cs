namespace _3dShop.Api.Models.Entities
{
    public class OrderItem : BaseEntity
    {
        public required string ProductNameSnapshot { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }
        public decimal ItemTotal { get; set; } 

        //Relations
        public required Guid OrderId { get; set; } //FK
        public required Guid ProductId { get; set; } //FK
        public required Order Order { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public required Product Product{ get; set; } //Navegação - Facilita acesso a entidade relacionada no código
    }
}
