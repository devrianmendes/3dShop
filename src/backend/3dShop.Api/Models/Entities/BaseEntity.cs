namespace _3dShop.Api.Models.Entities
{
    //Propriedades geradas pela aplicação não necessitam de required.
    //Required é utilizado somente quando O USUÁRIO precisa fornecer o dado.
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //Não precisa de required pois gera um valor ao instanciar
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //Não precisa de required pois gera um valor ao instanciar
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; //Não precisa de required pois gera um valor ao instanciar
    }
}
