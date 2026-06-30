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

        [HttpGet]
        [ProducesResponseType<GetAllProductsResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetAllProductsResponse>> GetAllProductsAsync()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        
        [HttpGet("{productId:Guid}", Name = "GetProductByIdAsync")]
        //[Authorize(Roles = "Admin, Seller")]
        [ProducesResponseType<GetProductResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetProductResponse>> GetProductByIdAsync([FromRoute] Guid productId)
        {
            if(productId == Guid.Empty)
            {
                throw new BadRequestException("Produto inválido.");
            }
            GetProductResponse product = await _productService.GetProductByIdAsync(productId);

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<GetProductListResponse>> GetAllProducts()
        {
            //var teste = await _productService.GetAllProductsServiceAsync();
            return Ok(await _productService.GetAllProductsServiceAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        [ProducesResponseType<CreateProductResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateProductResponse>> CreateProduct(CreateProductResquest createProductResquest)
        {
            _validator.ValidateAndThrow(createProductResquest);

            var createdProduct = await _productService.CreateProductServiceAsync(createProductResquest);

            return CreatedAtRoute(
                nameof(GetProductByIdAsync),
                new { productId = createdProduct.Id},
                createdProduct
            );
        }
    }
}
