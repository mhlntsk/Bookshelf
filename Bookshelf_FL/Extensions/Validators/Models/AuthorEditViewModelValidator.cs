using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.BookViewModels;
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
