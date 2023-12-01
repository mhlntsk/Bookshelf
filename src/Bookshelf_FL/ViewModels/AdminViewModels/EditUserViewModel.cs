using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.ViewModels.AdminViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Login { get; set; }
    }
}
