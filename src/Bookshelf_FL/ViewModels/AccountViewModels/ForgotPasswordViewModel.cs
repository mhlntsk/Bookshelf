using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
