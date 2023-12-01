using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        public string UsernameOrEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
