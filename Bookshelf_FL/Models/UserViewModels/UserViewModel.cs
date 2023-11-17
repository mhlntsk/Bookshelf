using Bookshelf_FL.Extensions.Services;
using Bookshelf_TL.Models;
using System.Security.AccessControl;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Bookshelf_FL.Models.BookViewModels;
using Microsoft.AspNetCore.Hosting;

namespace Bookshelf_FL.Models.UserViewModels
{
    public class UserViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public IEnumerable<BookListViewModel> ReadBooks { get; set; }
        public IEnumerable<BookListViewModel> CurrentlyReadingBooks { get; set; }
        public IEnumerable<BookListViewModel> WantToReadBooks { get; set; }
    }
}
