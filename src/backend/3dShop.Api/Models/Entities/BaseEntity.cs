namespace _3dShop.Api.Models.Entities
{
    //Propriedades geradas pela aplicação não necessitam de required pois o dado já será fornecido ao instanciar.
    //Required é utilizado somente quando o dado precisa vir de alguma fonte, seja usuário ou gerado em algum momento entre controller - registro no banco.
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //Não precisa de required pois gera um valor ao instanciar
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //Não precisa de required pois gera um valor ao instanciar
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; //Não precisa de required pois gera um valor ao instanciar
    }
}
