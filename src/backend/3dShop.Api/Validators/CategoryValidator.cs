using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Models.Interfaces;
using FluentValidation;

namespace _3dShop.Api.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryNamesBase>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.NamePt)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(255);

            RuleFor(c => c.NameEn)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(255);
        }
    }
}