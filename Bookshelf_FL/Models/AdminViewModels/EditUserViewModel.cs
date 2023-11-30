using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.AdminViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
    }
}
