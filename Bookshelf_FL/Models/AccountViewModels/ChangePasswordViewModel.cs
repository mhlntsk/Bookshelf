using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
