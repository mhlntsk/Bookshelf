using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AccountViewModels
{
    public class ResetForgottenPassword
    {
        [Required]
        public string Token { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
