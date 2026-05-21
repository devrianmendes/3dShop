namespace _3dShop.Api.Models.Entities
{
    public class CartItem : BaseEntity
    {
        public required string ProductNameSnapshot { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }

        //Relations
        public required Cart Cart { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public required Product Product { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public required Guid CartId { get; set; } //FK
        public required Guid ProductId { get; set; } //FK
    }
}
