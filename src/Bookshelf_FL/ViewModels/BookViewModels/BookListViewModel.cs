using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.ViewModels.AuthorViewModels;
using Bookshelf_FL.ViewModels.Categories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models.IntermediateModels;
using Bookshelf_TL.Models;
using Bookshelf_FL.ViewModels.UserViewModels;

namespace Bookshelf_FL.ViewModels.BookViewModels
{
    public class BookListViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string BookName { get; set; }
        public string Series { get; set; }
        public int? NumberInSeries { get; set; }
        public decimal? AverageBookScore { get; set; }
        public string CoverPath { get; set; }
        public string StatusOfUser { get; set; }
        public ICollection<CategoryDtoViewModel> Categories { get; set; }
        public ICollection<AuthorDtoViewModel> Authors { get; set; }
    }
}
