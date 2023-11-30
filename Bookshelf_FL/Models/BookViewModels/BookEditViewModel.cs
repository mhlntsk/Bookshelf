using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.Categories;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.BookViewModels
{
    public class BookEditViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string BookName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfPublication { get; set; }
        public string? Series { get; set; }
        public int? NumberInSeries { get; set; }
        public string? Description { get; set; }
        public string? CoverPath { get; set; }
        public IFormFile? CoverImage { get; set; }
        public ICollection<CategoryDtoViewModel>? Categories { get; set; }
        public ICollection<AuthorDtoViewModel>? Authors { get; set; }
        public List<string>? SelectedCategories { get; set; }
        public List<string>? SelectedAuthors { get; set; }
    }
}
