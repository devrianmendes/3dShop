using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateProductResponseDTOs> CreateProductServiceAsync(CreateProductResquestDTOs productData)
        {
            var productExist = await _context.Products.AnyAsync(p => p.NamePt.Trim().ToLower() == productData.NamePt.Trim().ToLower());

            if (productExist)
            {
                throw new BadRequestException("Produto já cadastrado.");
            }

            var categoryExist = await _context.Categories.AnyAsync(c => c.Id == productData.CategoryId);

            if (!categoryExist)
            {
                throw new NotFoundException("Categoria não existe.");
            }

            Product newProduct = new()
            {
                NamePt = productData.NamePt,
                NameEn = productData.NameEn,
                DescriptionPt = productData.DescriptionPt,
                DescriptionEn = productData.DescriptionEn,
                CategoryId = productData.CategoryId,
                IsActive = productData.IsActive,
                IsCustom = productData.IsCustom,
                Price = productData.Price,
                //ProductImageList = productData.ProductImageList
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return new CreateProductResponseDTOs(
                newProduct.Id
            );
        }
    }
}
