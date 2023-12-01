using Bookshelf_FL.ViewModels.AuthorViewModels;
using Bookshelf_FL.ViewModels.Categories;
using Bookshelf_TL.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.BookViewModels
{
    public class BookCreateViewModel
    {
        public string BookName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfPublication { get; set; }
        public string? Series { get; set; }
        public int? NumberInSeries { get; set; }
        public string? Description { get; set; }
        public IFormFile? CoverImage { get; set; }
        public ICollection<CategoryDtoViewModel>? Categories { get; set; }
        public ICollection<AuthorDtoViewModel>? Authors { get; set; }
        public List<string>? SelectedCategories { get; set; }
        public List<string>? SelectedAuthors { get; set; }
    }
}
