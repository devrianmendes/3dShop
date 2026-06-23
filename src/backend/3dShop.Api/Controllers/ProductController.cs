using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IValidator<CreateProductResquestDTOs> _validator;
        private readonly ProductService _productService;

        public ProductController(IValidator<CreateProductResquestDTOs> validator, ProductService productService)
        {
            _validator = validator;
            _productService = productService;
        }

        [HttpGet("{ProductId:Guid}", Name = "GetProductById")]
        public async Task<ActionResult<bool>> GetProductById()
        {
            return true;
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductResponseDTOs>> CreateProduct(CreateProductResquestDTOs createProductResquest)
        {
            _validator.ValidateAndThrow(createProductResquest);

            var createdProduct = await _productService.CreateProductServiceAsync(createProductResquest);

            return CreatedAtRoute(
                nameof(GetProductById),
                new { ProductId = createdProduct},
                createdProduct
            );
        }
    }
}
