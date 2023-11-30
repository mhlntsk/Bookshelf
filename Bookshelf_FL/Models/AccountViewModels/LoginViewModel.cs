using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AccountViewModels
{
    public class LoginViewModel
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; } 
    }
}
