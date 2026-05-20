namespace _3dShop.Api.Models.Entities
{
    //Propriedades geradas pela aplicação não necessitam de required.
    //Required é utilizado somente quando O USUÁRIO precisa fornecer o dado.
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
