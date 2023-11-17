using Bookshelf_TL.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf_FL.Models.UserViewModels
{
    public class UserEditViewModel : ICoverImageViewModel
    {
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Display(Name = "По-батькові / друге ім'я")]
        public string MiddleName { get; set; }

        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^\+\d{1,3}\(\d{1,3}\)\d{3}-\d{2}-\d{2}$", 
            ErrorMessage = "Невірно введений номер телефону. Притримуйтесь формату: \"+код країни(номер оператору)000-00-00\"")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Про себе")]
        public string Description { get; set; }

        [Display(Name = "Дата народження")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string CoverPath { get; set; }
        public IFormFile CoverImage { get; set; }
    }
}
