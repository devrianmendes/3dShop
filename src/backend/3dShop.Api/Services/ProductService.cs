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

        public async Task<GetProductResponse> GetProductById(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if(product is null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            return new GetProductResponse()
            {
                NamePt = product.NamePt,
                NameEn = product.NameEn,
                DescriptionPt = product.DescriptionPt,
                DescriptionEn = product.DescriptionEn,
                Price = product.Price,
                IsCustom = product.IsCustom,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                ProductImageList = product.ProductImageList,
            };
        }

        public async Task<CreateProductResponse> CreateProductServiceAsync(CreateProductResquest productData)
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
                ProductImageList = productData.ProductImageList
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return new CreateProductResponse()
            {
                Id = newProduct.Id
            };            
        }
    }
}
