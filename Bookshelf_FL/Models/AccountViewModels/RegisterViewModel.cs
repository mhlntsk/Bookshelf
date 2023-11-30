using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Age { get; set; }
    }
}
