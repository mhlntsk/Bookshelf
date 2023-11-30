using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AdminViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
