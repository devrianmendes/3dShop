using _3dShop.Api.Data;
using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs;
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

        public async Task<CreateCategoryResponse> CreateCategoryAsync(CategoryNamesBase newCategoryRequest, CancellationToken cancellationToken)
        { 
            var categoryExist = await _context.Categories.AnyAsync(c =>
            c.NamePt == newCategoryRequest.NamePt.Trim().ToLower() ||
            c.NameEn == newCategoryRequest.NameEn.Trim().ToLower(), cancellationToken);

            if (categoryExist)
            {
                throw new BadRequestException("Categoria já existe");
            }

            Category newCategory = new()
            {
                NamePt = newCategoryRequest.NamePt,
                NameEn = newCategoryRequest.NameEn,
            };

            await _context.Categories.AddAsync(newCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCategoryResponse()
            {
                Id = newCategory.Id,
                NameEn = newCategory.NameEn,
                NamePt = newCategory.NamePt
            };
        }

        public async Task<CategoryListResponse> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            return new CategoryListResponse()
            {
                CategoryList = await _context.Categories
                .AsNoTracking()
                .Select(e => new GetCategoryResponse()
                {
                    Id = e.Id,
                    NamePt = e.NamePt,
                    NameEn = e.NameEn
                })
                .ToListAsync(cancellationToken)
            };
        }

        public async Task<GetCategoryResponse> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var requestedCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

            if (requestedCategory is null)
            {
                throw new NotFoundException("Categoria não encontrada.");
            }

            return new GetCategoryResponse()
            {
                Id = requestedCategory.Id,
                NameEn = requestedCategory.NameEn,
                NamePt = requestedCategory.NamePt,
            };
        }

        public async Task<UpdateCategoryResponse> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException("Categoria não encontrada.");
            }

            var categoryWithSameName = await _context.Categories.AnyAsync(c => 
                c.Id != categoryId && (
                    c.NamePt.Trim().ToLower() == updateCategoryRequest.NamePt.Trim().ToLower() || 
                    c.NameEn.Trim().ToLower() == updateCategoryRequest.NameEn.Trim().ToLower()
                ),
                cancellationToken);

            if(categoryWithSameName)
            {
                throw new BadRequestException("Já existe uma categoria com esse nome.");
            }

            category.NamePt = updateCategoryRequest.NamePt;
            category.NameEn = updateCategoryRequest.NameEn;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryResponse()
            {
                Id = category.Id,
                NameEn = category.NameEn,
                NamePt = category.NamePt
            };
        }

        public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken); //Não é necessário carregar os produtos da categoria para verificar se há produtos cadastrados, basta verificar se algum produto tem o id da categoria como FK

            if (category is null)
            {
                throw new NotFoundException("Categoria não encontrada.");
            }

            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == categoryId, cancellationToken); //Não é necessário carregar os produtos da categoria para verificar se há produtos cadastrados, basta verificar se algum produto tem o id da categoria como FK

            if(hasProducts)
            {
                throw new BadRequestException("Não é possível deletar uma categoria com produtos registrados.");
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}