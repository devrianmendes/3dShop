namespace _3dShop.Api.Models.Entities
{
    public class RefreshToken : BaseEntity
    {
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? DeviceId { get; set; }
        public string?  UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public User User { get; set; } = null!;
    }
}
