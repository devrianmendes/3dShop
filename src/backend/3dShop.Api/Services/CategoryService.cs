using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs.Category;
using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3dShop.Api.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetCategoryByIdResponse> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadRequestException("Id inválido");
            }

            var requestedCategory = await _context.Categories.Include(c => c.ProductList)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (requestedCategory is null)
            {
                throw new NotFoundException("Categoria não encontrada.");
            }

            return new GetCategoryByIdResponse()
            {
                Id = requestedCategory.Id,
                NameEn = requestedCategory.NameEn,
                NamePt = requestedCategory.NamePt,
                ProductList = requestedCategory.ProductList
            };
        }

        public async Task<NewCategoryResponse> CreateCategory(NewCategoryRequest newCategoryRequest, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(newCategoryRequest.NamePt) || String.IsNullOrWhiteSpace(newCategoryRequest.NameEn))
            {
                throw new BadRequestException("O nome da categoria não podem estar vazios.");
            }

            var categoryExist = await _context.Categories.AnyAsync(c =>
            c.NamePt == newCategoryRequest.NamePt.Trim().ToLower() ||
            c.NameEn == newCategoryRequest.NameEn.Trim().ToLower());

            if (categoryExist)
            {
                throw new BadRequestException("Categoria já existe");
            }

            Category newCategory = new()
            {
                NamePt = newCategoryRequest.NamePt.Trim().ToLower(),
                NameEn = newCategoryRequest.NameEn.Trim().ToLower(),
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return new NewCategoryResponse()
            {
                Id = newCategory.Id,
                NameEn = newCategory.NameEn,
                NamePt = newCategory.NamePt
            };
        }
    }

}