namespace _3dShop.Api.Models.DTOs
{
    public record RefreshTokenRequest
    {
        public required string RefreshToken { get; set; }
        public Guid? DeviceId { get; set; }
    }

    public record RefreshTokenResponse
    {
        public required string newRefreshToken { get; set; }
        public required string newAccessToken { get; set; }
        public required DateTime ExpirationDate { get; set; }
    }
}
