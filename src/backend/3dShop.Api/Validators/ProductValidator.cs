using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Entities;
using FluentValidation;

namespace _3dShop.Api.Validators
{
    public class ProductValidator : AbstractValidator<CreateProductResquestDTOs>
    {
        public ProductValidator()
        {
            RuleFor(p => p.NamePt)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(255)
                .WithMessage("O nome deve ter mais do que 4 caracteres.");

            RuleFor(p => p.NameEn)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(255)
                .WithMessage("The name must have more than 4 characters.");

            RuleFor(p => p.DescriptionPt)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(2000)
                .WithMessage("O nome deve ter mais do que 4 caracteres.");

            RuleFor(p => p.DescriptionEn)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(2000)
                .WithMessage("The name must have more than 4 characters.");


            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("O valor deve ser maior do que 0.");

            RuleFor(p => p.CategoryId)
                .NotEmpty();
        }
    }
}
