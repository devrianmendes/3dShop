using _3dShop.Api.Exceptions;
using _3dShop.Api.Models.DTOs.Category;
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
        private readonly IValidator<NewCategoryRequest> _validate;
        private readonly CategoryService _categoryService;
        public CategoryController(IValidator<NewCategoryRequest> validate, CategoryService categoryService)
        {
            _validate = validate;
            _categoryService = categoryService;
        }

        //[Authorize(Roles = "Admin, Seller, Customer")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [ProducesResponseType<SingleCategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            IEnumerable<SingleCategoryResponse> categories = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(categories);
        }

        [Authorize(Roles = "Admin, Seller, Customer")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [ProducesResponseType<SingleCategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{CategoryId}", Name = "GetByIdAsync")]
        public async Task<ActionResult> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BadRequestException("Id inválido");
            }

            SingleCategoryResponse category = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);

            return Ok(category);
        }

        [Authorize(Roles = "Admin, Seller")] //UseAuthorization só permitira acesso a essa rota por pessoas logadas, com tokens validos e com roles permitidas
        [HttpPost]
        [ProducesResponseType<NewCategoryResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<NewCategoryResponse>> CreateCategoryAsync(NewCategoryRequest newCategoryRequest, CancellationToken cancellationToken)
        {
            _validate.ValidateAndThrow(newCategoryRequest);

            NewCategoryResponse newCategoryResponse = await _categoryService.CreateCategoryAsync(newCategoryRequest, cancellationToken);

            return CreatedAtRoute( //Por algum motivo, createdAtAction não funcionou de jeito nenhum
                nameof(GetByIdAsync),
                new { CategoryId = newCategoryResponse.Id }, 
                newCategoryResponse
            );
        }
    }
}