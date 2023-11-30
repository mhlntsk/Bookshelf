using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AdminViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
