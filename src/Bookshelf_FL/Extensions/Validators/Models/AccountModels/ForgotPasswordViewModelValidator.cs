using Bookshelf_FL.ViewModels.AccountViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models.AccountModels
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .EmailAddress().WithMessage("Enter a valid email address.");
        }
    }
}
