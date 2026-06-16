using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs;
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
        private readonly IValidator<CategoryNamesBase> _validate;
        private readonly CategoryService _categoryService;
        public CategoryController(IValidator<CategoryNamesBase> validate, CategoryService categoryService)
        {
            _validate = validate;
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin, Seller")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [HttpPost]
        [ProducesResponseType<CreateCategoryResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CreateCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest categoryRequest, CancellationToken cancellationToken)
        {
            await _validate.ValidateAndThrowAsync(categoryRequest, cancellationToken);

            CreateCategoryResponse categoryResponse = await _categoryService.CreateCategoryAsync(categoryRequest, cancellationToken);

            return CreatedAtRoute( //Por algum motivo, createdAtAction não funcionou de jeito nenhum
                nameof(GetByIdAsync),
                new { CategoryId = categoryResponse.Id },
                categoryResponse
            );
        }

        //[Authorize(Roles = "Admin, Seller, Customer")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [ProducesResponseType<CategoryListResponse>(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            CategoryListResponse categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);

            return Ok(categories);
        }

        [Authorize(Roles = "Admin, Seller, Customer")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [ProducesResponseType<GetCategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{CategoryId:guid}", Name = "GetByIdAsync")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] Guid categoryId, CancellationToken cancellationToken)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BadRequestException("Id inválido");
            }

            GetCategoryResponse category = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);

            return Ok(category);
        }

        [Authorize(Roles = "Admin, Seller")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [HttpPut("{categoryId:guid}")] //:guid garante que o Guid passado tem formato válido
        [ProducesResponseType<UpdateCategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategoryAsync(
            [FromRoute] Guid categoryId, 
            [FromBody] UpdateCategoryRequest updateCategoryRequest, 
            CancellationToken cancellationToken)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BadRequestException("Id inválido");
            }

            await _validate.ValidateAndThrowAsync(updateCategoryRequest, cancellationToken);

            UpdateCategoryResponse newCategoryResponse = await _categoryService.UpdateCategoryAsync(categoryId, updateCategoryRequest, cancellationToken);

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