using _3dShop.Api.Models.Entities;

public class GetCategoryByIdResponse
{
    public required Guid Id {get; set;}
    public required string NamePt { get; set; }
    public required string NameEn { get; set; }

    public ICollection<Product>? ProductList { get; set; }
}