using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Interfaces;
using _3dShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace _3dShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IValidator<IValidateCategory> _validate;
        private readonly CategoryService _categoryService;
        public CategoryController(IValidator<IValidateCategory> validate, CategoryService categoryService)
        {
            _validate = validate;
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin, Seller")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [HttpPost]
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CategoryResponse>> CreateCategoryAsync(CategoryRequest CategoryRequest, CancellationToken cancellationToken)
        {
            _validate.ValidateAndThrow(CategoryRequest);

            CategoryResponse CategoryResponse = await _categoryService.CreateCategoryAsync(CategoryRequest, cancellationToken);

            return CreatedAtRoute( //Por algum motivo, createdAtAction não funcionou de jeito nenhum
                nameof(GetByIdAsync),
                new { CategoryId = CategoryResponse.Id },
                CategoryResponse
            );
        }

        //[Authorize(Roles = "Admin, Seller, Customer")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            IEnumerable<CategoryResponse> categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);

            return Ok(categories);
        }

        [Authorize(Roles = "Admin, Seller, Customer")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{CategoryId:guid}", Name = "GetByIdAsync")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] Guid categoryId, CancellationToken cancellationToken)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BadRequestException("Id inválido");
            }

            CategoryResponse category = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);

            return Ok(category);
        }

        [Authorize(Roles = "Admin, Seller")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [HttpPut("{categoryId:guid}")] //:guid garante que o Guid passado tem formato válido
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(
            [FromRoute] Guid categoryId, 
            [FromBody] UpdateCategoryRequest updateCategoryRequest, 
            CancellationToken cancellationToken)
        {
            updateCategoryRequest.Id = categoryId;

            //_validate.ValidateAndThrow(updateCategoryRequest);

            CategoryResponse newCategoryResponse = await _categoryService.UpdateCategoryAsync(updateCategoryRequest, cancellationToken);

            return Ok(newCategoryResponse);
        }

        [Authorize(Roles = "Admin, Seller")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [HttpDelete("{categoryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteCategoryAsync([FromRoute] Guid categoryId, CancellationToken cancellationToken)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BadRequestException("Id inválido");
            }

            await _categoryService.DeleteCategoryAsync(categoryId, cancellationToken);

            return NoContent();            
        }
    }
}