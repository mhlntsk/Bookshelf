using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AdminViewModels
{
    public class CreateUserViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Login { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
