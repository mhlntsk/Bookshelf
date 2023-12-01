using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshelf_TL.Models
{
    public class Book : IEntity
    {
        [Required, Key]
        public string Id { get; set; }
        public string BookName { get; set; }
        public string Series { get; set; }
        public int? NumberInSeries { get; set; }
        public string Description { get; set; }
        public decimal? AverageBookScore { get; set; }
        public string CoverPath { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfPublication { get; set; }

        [InverseProperty("Book")]
        public ICollection<BookCategory> BookCategories { get; set; }

        [InverseProperty("Book")]
        public ICollection<BookAuthor> BookAuthors { get; set; }

        [InverseProperty("Book")]
        public ICollection<BookUser> BookUsers { get; set; }

        public string GetDefaultCover()
        {
            const string defaultCoverOfBook = "~/defaultFiles/DefaultBook.jpg";

            return defaultCoverOfBook;
        }
    }
}