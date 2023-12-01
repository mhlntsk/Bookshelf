using Bookshelf_FL.ViewModels.UserViewModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Bookshelf_FL.Extensions.Validators.Models
{
    public class UserEditViewModelValidator : AbstractValidator<UserEditViewModel>
    {
        public UserEditViewModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The 'FirstName' field is required.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("The 'UserName' field is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .EmailAddress().WithMessage("Enter a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .Empty().When(x => string.IsNullOrEmpty(x.PhoneNumber))
                .Matches(@"^\+\d{1,3}\(\d{1,3}\)\d{3}-\d{2}-\d{2}")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Invalid phone number format.");
        }
    }
}
