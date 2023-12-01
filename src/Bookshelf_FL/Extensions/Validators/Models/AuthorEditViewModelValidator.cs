using Bookshelf_FL.ViewModels.AuthorViewModels;
using Bookshelf_FL.ViewModels.BookViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models
{
    public class AuthorEditViewModelValidator : AbstractValidator<AuthorEditViewModel>
    {
        public AuthorEditViewModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The 'FirstName' field is required")
                .WithName("Name of Author");
        }
    }
}
