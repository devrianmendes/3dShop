using _3dShop.Api.Models.Interfaces;
using FluentValidation;

namespace _3dShop.Api.Validators
{
    //Dois validators para validar separadamente categories com e sem ID
    public class CategoryValidator : AbstractValidator<IValidateCategory>
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

    public class CategoryValidatorWithId : AbstractValidator<IValidateCategoryWithId>
    {
        public CategoryValidatorWithId()
        {
            Include(new CategoryValidator()); //Dessa forma podemos utilizar as validações dos nomes sem precisar reescreve-los novamente aqui

            RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}
