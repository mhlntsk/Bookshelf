using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
