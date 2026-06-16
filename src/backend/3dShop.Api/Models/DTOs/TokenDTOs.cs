namespace _3dShop.Api.Models.DTOs
{
    public record RefreshTokenRequest
    {
        public string? RefreshToken { get; set; }
        public required Guid DeviceId { get; set; }
    }

    public record RefreshTokenResponse
    {
        public required string RefreshToken { get; set; }
    }
}
