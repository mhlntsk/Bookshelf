using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Login { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int Age { get; set; }
    }
}
