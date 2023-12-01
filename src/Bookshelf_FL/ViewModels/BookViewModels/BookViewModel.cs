using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.ViewModels.AuthorViewModels;
using Bookshelf_FL.ViewModels.Categories;
using Bookshelf_FL.ViewModels.UserViewModels;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.BookViewModels
{
    public class BookViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string BookName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateOfPublication { get; set; }
        public string Series { get; set; }
        public int? NumberInSeries { get; set; }
        public string CoverPath { get; set; }
        public string Description { get; set; }
        public string StatusOfCurrentUser { get; set; }
        public decimal? AverageBookScore { get; set; }
        public int? IndividualBookScore { get; set; }
        public ICollection<AuthorDtoViewModel> Authors{ get; set; }
        public ICollection<CategoryDtoViewModel> Categories { get; set; }

        
    }
}
