using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshelf_TL.Models
{
    public class Author : IEntity
    {
        [Required, Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string CoverPath { get; set; }
        public string Description { get; set; }

        [InverseProperty("Author")]
        public ICollection<BookAuthor> BookAuthors { get; set; }

        public string GetDefaultCover()
        {
            const string defaultCoverOfAuthor = "~/defaultFiles/DefaultAuthor.jpg";

            return defaultCoverOfAuthor;
        }
    }
}
