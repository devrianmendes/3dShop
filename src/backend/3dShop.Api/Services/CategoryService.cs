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

        public async Task<IEnumerable<SingleCategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(e => new SingleCategoryResponse()
                    {
                        Id = e.Id,
                        NamePt = e.NamePt,
                        NameEn = e.NameEn
                    })
                .ToListAsync(cancellationToken);
        }


        public async Task<SingleCategoryResponse> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var requestedCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

            if (requestedCategory is null)
            {
                throw new NotFoundException("Categoria não encontrada.");
            }

            return new SingleCategoryResponse()
            {
                Id = requestedCategory.Id,
                NameEn = requestedCategory.NameEn,
                NamePt = requestedCategory.NamePt,
                //ProductList = requestedCategory.ProductList
            };
        }

        public async Task<NewCategoryResponse> CreateCategoryAsync(NewCategoryRequest newCategoryRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newCategoryRequest.NamePt) || string.IsNullOrWhiteSpace(newCategoryRequest.NameEn))
            {
                throw new BadRequestException("O nome da categoria não podem estar vazios.");
            }

            var categoryExist = await _context.Categories.AnyAsync(c =>
            c.NamePt == newCategoryRequest.NamePt.Trim().ToLower() ||
            c.NameEn == newCategoryRequest.NameEn.Trim().ToLower(), cancellationToken);

            if (categoryExist)
            {
                throw new BadRequestException("Categoria já existe");
            }

            Category newCategory = new()
            {
                NamePt = newCategoryRequest.NamePt.Trim().ToLower(),
                NameEn = newCategoryRequest.NameEn.Trim().ToLower(),
            };

            await _context.Categories.AddAsync(newCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new NewCategoryResponse()
            {
                Id = newCategory.Id,
                NameEn = newCategory.NameEn,
                NamePt = newCategory.NamePt
            };
        }
    }

}