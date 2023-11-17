using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshelf_TL.Models
{
    public class User : IdentityUser, IEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [InverseProperty("User")]
        public ICollection<BookUser> BookUsers { get; set; }

        public string GetDefaultCover()
        {
            const string defaultCoverOfUser = "~/defaultFiles/DefaultUser.jpg";

            return defaultCoverOfUser;
        }

    }
}
