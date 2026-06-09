namespace _3dShop.Api.Models.DTOs
{
    // --- Bases ---
    public abstract record CategoryNamesBase
    {
        public required string NamePt { get; init; }
        public required string NameEn { get; init; }
    }

    public abstract record CategoryResponseBase : CategoryNamesBase
    {
        public required Guid Id { get; init; }
    }

    // --- Requests ---

    public record CreateCategoryRequest : CategoryNamesBase;
    public record UpdateCategoryRequest : CategoryNamesBase;
    public record DeleteCategoryRequest
    {
        public required Guid Id { get; init; }
    }

    // --- Responses ---
    public record GetCategoryResponse : CategoryResponseBase;
    public record CreateCategoryResponse : CategoryResponseBase;
    public record UpdateCategoryResponse : CategoryResponseBase;

    public class CategoryListResponse
    {
        public IEnumerable<GetCategoryResponse> CategoryList { get; set; } = [];
    }
}