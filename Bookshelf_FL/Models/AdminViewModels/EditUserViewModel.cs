using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AdminViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Login")]
        public string Login { get; set; }
    }
}
