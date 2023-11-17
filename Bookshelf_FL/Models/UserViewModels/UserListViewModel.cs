using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_TL.Models;

namespace Bookshelf_FL.Models.UserViewModels
{
    public class UserListViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string CoverPath { get; set; }
    }
}
