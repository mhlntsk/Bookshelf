using Bookshelf_FL.ViewModels.AuthorViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models
{
    public class AuthorCreateViewModelValidator : AbstractValidator<AuthorCreateViewModel>
    {
        public AuthorCreateViewModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The 'FirstName' field is required")
                .WithName("Name of Author");
        }
    }
}
