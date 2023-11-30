using Bookshelf_FL.Models.AccountViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models.AccountModels
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The 'Password' field is required.")
                .MinimumLength(8).WithMessage("Min password length is 8")
                .Matches("[A-Z]").WithMessage("The password must contain at least one capital letter")
                .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("The password must contain at least one number")
                .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("The password must contain at least one special character");
        }
    }
}
