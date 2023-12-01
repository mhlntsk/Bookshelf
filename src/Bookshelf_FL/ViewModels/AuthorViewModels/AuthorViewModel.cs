using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.ViewModels.BookViewModels;
using Bookshelf_TL.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Bookshelf_FL.ViewModels.AuthorViewModels
{
    public class AuthorViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string CoverPath { get; set; }
        public string Description { get; set; }
        public ICollection<BookListViewModel> SelectedBooks { get; set; }
    }
}
