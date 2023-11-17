using Bookshelf_FL.Models.BookViewModels;
using Bookshelf_FL.Models.UserViewModels;
using Bookshelf_SL.Repositories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;

namespace Bookshelf_FL.Models.AuthorViewModels
{
    public class AuthorEditViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }
        public IFormFile CoverImage { get; set; }
        public ICollection<BookLiteViewModel> Books { get; set; }
        public List<string> SelectedBooks { get; set; }
    }
}
