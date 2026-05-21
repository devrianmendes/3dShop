namespace _3dShop.Api.Models.Entities
{
    public class Category : BaseEntity
    {
        public required string NamePt { get; set; }
        public required string NameEn { get; set; }

        //Relations
        public ICollection<Product>? ProductList { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
    }
}
