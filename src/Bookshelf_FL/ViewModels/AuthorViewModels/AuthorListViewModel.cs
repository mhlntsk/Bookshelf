using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.ViewModels.BookViewModels;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshelf_FL.ViewModels.AuthorViewModels
{
    public class AuthorListViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string CoverPath { get; set; }
    }
}
