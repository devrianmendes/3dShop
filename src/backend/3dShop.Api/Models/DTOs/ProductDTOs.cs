using _3dShop.Api.Models.Entities;

namespace _3dShop.Api.Models.DTOs
{
    public record CreateProductResquestDTOs(
        string NamePt, 
        string NameEn, 
        string DescriptionPt, 
        string DescriptionEn, 
        decimal Price, 
        bool IsCustom, 
        bool IsActive, 
        Guid CategoryId
        //ICollection<ProductImage>? ProductImageList
    );

    public record CreateProductResponseDTOs(
        Guid Id
    );
}
