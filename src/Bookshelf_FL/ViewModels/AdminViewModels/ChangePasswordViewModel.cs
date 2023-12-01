using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AdminViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
