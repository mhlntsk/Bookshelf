using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshelf_TL.Models.IntermediateModels
{
    public class BookAuthor : IIntermediateEntity
    {
        [Key]
        public string BookId { get; set; }

        [Key]
        public string AuthorId { get; set; }


        [ForeignKey("BookId"), Required]
        public Book Book { get; set; }

        [ForeignKey("AuthorId"), Required]
        public Author Author { get; set; }
    }
}