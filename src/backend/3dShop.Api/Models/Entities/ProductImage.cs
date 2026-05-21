namespace _3dShop.Api.Models.Entities
{
    public class ProductImage : BaseEntity
    {
        public required string Url { get; set; }
        public required bool IsMain { get; set; }
        public required int DisplayOrder { get; set; }

        //Relations
        public required Guid ProductId { get; set; } //FK
        public required Product Product { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
    }
}
