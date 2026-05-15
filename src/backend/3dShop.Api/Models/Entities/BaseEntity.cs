namespace _3dShop.Api.Models.Entities
{
    public class BaseEntity
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
