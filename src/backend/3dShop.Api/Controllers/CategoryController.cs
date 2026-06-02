using _3dShop.Api.Models.DTOs.Category;
using _3dShop.Api.Services;
using _3dShop.Api.Validators;
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
        private readonly CategoryService _createCategoryService;
        public CategoryController(IValidator<NewCategoryRequest> validate, CategoryService createCategoryService)
        {
            _validate = validate;
            _createCategoryService = createCategoryService;
        }

        [HttpGet("{CategoryId}", Name = "GetByIdAsync")]
        public IActionResult GetByIdAsync(Guid CategoryId)
        {
            return Ok();
        }

        // [Authorize(Roles = "Admin, Seller")]
        [HttpPost]
        [ProducesResponseType<NewCategoryResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NewCategoryResponse>> CreateCategory(NewCategoryRequest newCategoryRequest, CancellationToken cancellationToken)
        {
            _validate.ValidateAndThrow(newCategoryRequest);

            NewCategoryResponse newCategoryResponse = await _createCategoryService.CreateCategory(newCategoryRequest, cancellationToken);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { CategoryId = newCategoryResponse.Id }, // Substitua 'Id' pelo nome da propriedade do ID no seu response
                newCategoryResponse
            );
        }
    }
}