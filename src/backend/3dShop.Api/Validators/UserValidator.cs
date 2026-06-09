using _3dShop.Api.Models.DTOs.Users;
using _3dShop.Api.Models.Interfaces;
using FluentValidation;

namespace _3dShop.Api.Validators
{
    public class UserValidator : AbstractValidator<IValidateUser>
    {
        public UserValidator()
        {
            RuleFor(aur => aur.Email)
                .EmailAddress()
                .MinimumLength(3)
                .MaximumLength(255);

            RuleFor(aur => aur.Password)
                .MinimumLength(8)
                .MaximumLength(16)
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$");
        }
    }
}
