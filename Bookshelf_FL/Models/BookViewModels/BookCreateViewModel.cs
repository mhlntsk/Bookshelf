using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.Categories;
using Bookshelf_TL.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.BookViewModels
{
    public class BookCreateViewModel
    {
        public string BookName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfPublication { get; set; }
        public string Series { get; set; }
        public int? NumberInSeries { get; set; }
        public string Description { get; set; }
        public IFormFile CoverImage { get; set; }
        public ICollection<CategoryLiteViewModel> Categories { get; set; }
        public ICollection<AuthorLiteViewModel> Authors { get; set; }
        public List<string> SelectedCategories { get; set; }
        public List<string> SelectedAuthors { get; set; }
    }
}
