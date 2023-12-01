using Bookshelf_TL.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.UserViewModels
{
    public class UserEditViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? CoverPath { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
