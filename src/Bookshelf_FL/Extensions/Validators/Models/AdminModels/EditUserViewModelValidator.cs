using Bookshelf_FL.ViewModels.AdminViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models.AdminModels
{
    public class EditUserViewModelValidator : AbstractValidator<EditUserViewModel>
    {
        public EditUserViewModelValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("The 'UserName' field is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .EmailAddress().WithMessage("Enter a valid email address.");
        }
    }
}
