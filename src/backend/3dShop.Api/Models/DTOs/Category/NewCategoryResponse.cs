namespace _3dShop.Api.Models.DTOs.Category
{
    public class NewCategoryResponse
    {
        public required Guid Id { get; set; }
        public required string NamePt { get; set; }
        public required string NameEn { get; set; }
    }
}