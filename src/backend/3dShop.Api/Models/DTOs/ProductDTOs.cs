using _3dShop.Api.Models.Entities;

namespace _3dShop.Api.Models.DTOs
{
    public record BaseProductRequest
    {
        public Guid Id { get; init; }
    }

    public record BaseProductResponse
    {
        public required Guid Id { get; init; }
        public required string NamePt { get; init; }
        public required string NameEn { get; init; }
        public required string DescriptionPt { get; init; }
        public required string DescriptionEn { get; init; }
        public required decimal Price { get; init; }
        public required bool IsCustom { get; init; }
        public required bool IsActive { get; init; }
        public required Guid CategoryId { get; init; }
        public ICollection<ProductImage>? ProductImageList { get; init; }
    };

    public record CreateProductResquest : BaseProductResponse;
    public record CreateProductResponse : BaseProductRequest;
    public record GetProductRequest : BaseProductRequest;
    public record GetProductResponse : BaseProductResponse;

    public record GetProductListResponse
    {
        public required IEnumerable<GetProductResponse> Products { get; init; }
    }
}
