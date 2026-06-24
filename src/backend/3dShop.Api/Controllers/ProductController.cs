using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using _3dShop.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IValidator<CreateProductResquest> _validator;
        private readonly ProductService _productService;

        public ProductController(IValidator<CreateProductResquest> validator, ProductService productService)
        {
            _validator = validator;
            _productService = productService;
        }

        [HttpGet("{productId:Guid}", Name = "GetProductById")]
        public async Task<ActionResult<GetProductResponse>> GetProductById([FromRoute] Guid productId)
        {
            if(productId == Guid.Empty)
            {
                throw new BadRequestException("Produto inválido.");
            }
            GetProductResponse product = await _productService.GetProductById(productId);

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        [ProducesResponseType<CreateProductResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<CreateProductResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<CreateProductResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<CreateProductResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateProductResponse>> CreateProduct(CreateProductResquest createProductResquest)
        {
            _validator.ValidateAndThrow(createProductResquest);

            var createdProduct = await _productService.CreateProductServiceAsync(createProductResquest);

            return CreatedAtRoute(
                nameof(GetProductById),
                new { productId = createdProduct},
                createdProduct
            );
        }
    }
}
