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
        private readonly IValidator<BaseProduct> _validator;
        private readonly ProductService _productService;

        public ProductController(IValidator<BaseProduct> validator, ProductService productService)
        {
            _validator = validator;
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType<GetProductListResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetProductListResponse>> GetAllProductsAsync()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        
        [HttpGet("{productId:Guid}", Name = "GetProductByIdAsync")]
        //[Authorize(Roles = "Admin, Seller")]
        [ProducesResponseType<GetProductResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetProductResponse>> GetProductByIdAsync([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            if(productId == Guid.Empty)
            {
                throw new BadRequestException("Produto inválido.");
            }
            GetProductResponse product = await _productService.GetProductByIdAsync(productId, cancellationToken);

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        [ProducesResponseType<CreateProductResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateProductResponse>> CreateProduct(CreateProductResquest createProductResquest, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(createProductResquest);

            var createdProduct = await _productService.CreateProductServiceAsync(createProductResquest, cancellationToken);

            return CreatedAtRoute(
                nameof(GetProductByIdAsync),
                new { productId = createdProduct.Id},
                createdProduct
            );
        }

        [HttpPut]
        public async Task<ActionResult<UpdateProductResponse>> UpdateProductAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
        {
            if(updateProductRequest.Id == Guid.Empty)
            {
                throw new BadRequestException("Produto inválido.");
            }

            _validator.ValidateAndThrow(updateProductRequest);

            return Ok(await _productService.UpdateProductAsync(updateProductRequest, cancellationToken));
        }
    }
}
