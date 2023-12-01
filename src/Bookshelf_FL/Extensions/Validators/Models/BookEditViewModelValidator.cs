using Bookshelf_FL.ViewModels.BookViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models
{
    public class BookEditViewModelValidator : AbstractValidator<BookEditViewModel>
    {
        public BookEditViewModelValidator()
        {
            RuleFor(x => x.BookName)
                .NotEmpty().WithMessage("The 'BookName' field is required")
                .WithName("Name of the book");

            RuleFor(x => x.NumberInSeries)
                .NotNull().NotEmpty().When(x => !string.IsNullOrEmpty(x.Series))
                .WithMessage("NumberInSeries is required when Series is specified.");
        }
    }
}
