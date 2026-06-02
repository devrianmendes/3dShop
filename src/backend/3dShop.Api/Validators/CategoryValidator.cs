using _3dShop.Api.Models.DTOs.Category;
using _3dShop.Api.Models.Entities;
using FluentValidation;

namespace _3dShop.Api.Validators
{
    public class CategoryValidator : AbstractValidator<NewCategoryRequest>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.NamePt)
                .MinimumLength(4)
                .MaximumLength(255);

            RuleFor(c => c.NameEn)
                .MinimumLength(4)
                .MaximumLength(255);
        }
    }
}
