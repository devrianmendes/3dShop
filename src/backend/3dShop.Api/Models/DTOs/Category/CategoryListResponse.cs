namespace _3dShop.Api.Models.DTOs.Category
{
    public class CategoryListResponse
    {
        public IEnumerable<SingleCategoryResponse> CategoryList { get; set; } = [];
    }
}
