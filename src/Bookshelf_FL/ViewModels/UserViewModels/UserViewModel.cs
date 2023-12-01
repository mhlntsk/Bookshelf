using Bookshelf_FL.ViewModels.BookViewModels;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.UserViewModels
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
