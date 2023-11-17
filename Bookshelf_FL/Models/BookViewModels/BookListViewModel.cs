using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.Categories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models.IntermediateModels;
using Bookshelf_TL.Models;
using Bookshelf_FL.Models.UserViewModels;

namespace Bookshelf_FL.Models.BookViewModels
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
        public ICollection<CategoryLiteViewModel> Categories { get; set; }
        public ICollection<AuthorLiteViewModel> Authors { get; set; }
    }
}
