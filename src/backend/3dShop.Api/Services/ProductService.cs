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

        public async Task<GetProductListResponse> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            return new GetProductListResponse()
            {
                Products = await _context.Products.Select(e => new GetProductResponse()
                {
                    Id = e.Id,
                    NamePt = e.NamePt,
                    NameEn = e.NameEn,
                    DescriptionPt = e.DescriptionPt,
                    DescriptionEn = e.DescriptionEn,
                    Price = e.Price,
                    IsCustom = e.IsCustom,
                    IsActive = e.IsActive,
                    CategoryId = e.CategoryId,
                    ProductImageList = e.ProductImageList,
                }).ToListAsync(cancellationToken)
            };
        }

        public async Task<GetProductResponse> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            return new GetProductResponse()
            {
                Id = product.Id,
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

        public async Task<CreateProductResponse> CreateProductServiceAsync(CreateProductResquest productData, CancellationToken cancellationToken)
        {
            var productExist = await _context.Products.AsNoTracking().AnyAsync(p => p.NamePt.Trim().ToLower() == productData.NamePt.Trim().ToLower(), cancellationToken);

            if (productExist)
            {
                throw new BadRequestException("Produto já cadastrado.");
            }

            var categoryExist = await _context.Categories.AsNoTracking().AnyAsync(c => c.Id == productData.CategoryId, cancellationToken);

            if (!categoryExist)
            {
                throw new NotFoundException("Categoria não existe.");
            }

            Product newProduct = new()
            {
                Id = Guid.NewGuid(),
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

            await _context.Products.AddAsync(newProduct, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateProductResponse()
            {
                Id = newProduct.Id
            };
        }

        public async Task<UpdateProductResponse> UpdateProductAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
        {
            var productExist = await _context.Products.AsNoTracking().FirstOrDefaultAsync(e => e.Id == updateProductRequest.Id, cancellationToken);

            if (productExist is null) throw new NotFoundException("Produto não encontrado.");

            var productWithSameName = await _context.Products.AsNoTracking().AnyAsync(e => e.Id != updateProductRequest.Id && (
                e.NameEn.Trim().ToLowerInvariant() == updateProductRequest.NameEn.Trim().ToLowerInvariant() ||
                e.NamePt.Trim().ToLowerInvariant() == updateProductRequest.NamePt.Trim().ToLowerInvariant()
            ), cancellationToken);

            if (productWithSameName is true) throw new BadRequestException("Nome já utilizado.");

            productExist.Id = updateProductRequest.Id;
            productExist.NamePt = updateProductRequest.NamePt;
            productExist.NameEn = updateProductRequest.NameEn;
            productExist.DescriptionPt = updateProductRequest.DescriptionPt;
            productExist.DescriptionEn = updateProductRequest.DescriptionEn;
            productExist.Price = updateProductRequest.Price;
            productExist.IsActive = updateProductRequest.IsActive;
            productExist.IsCustom = updateProductRequest.IsCustom;
            productExist.CategoryId = updateProductRequest.CategoryId;
            productExist.ProductImageList = updateProductRequest.ProductImageList;

            await _context.SaveChangesAsync(cancellationToken);

            return (new UpdateProductResponse()
            {
                Id = updateProductRequest.Id,
                NamePt = updateProductRequest.NamePt,
                NameEn = updateProductRequest.NameEn,
                DescriptionPt = updateProductRequest.DescriptionPt,
                DescriptionEn = updateProductRequest.DescriptionEn,
                Price = updateProductRequest.Price,
                IsActive = updateProductRequest.IsActive,
                IsCustom = updateProductRequest.IsCustom,
                CategoryId = updateProductRequest.CategoryId,
                ProductImageList = updateProductRequest.ProductImageList,
            });
        }
    }
}
