using Bookshelf_FL.Models.BookViewModels;

namespace Bookshelf_FL.Models.AuthorViewModels
{
    public class AuthorEditViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? CoverPath { get; set; }
        public IFormFile? CoverImage { get; set; }
        public ICollection<BookDtoViewModel>? Books { get; set; }
        public List<string>? SelectedBooks { get; set; }
    }
}
