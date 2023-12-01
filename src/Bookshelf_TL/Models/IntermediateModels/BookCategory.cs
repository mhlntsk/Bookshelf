using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf_TL.Models.IntermediateModels
{
    public class BookCategory : IIntermediateEntity
    {
        [Key]
        public string BookId { get; set; }

        [Key] 
        public string CategoryId { get; set; }

        [ForeignKey("BookId"), Required]
        public Book Book { get; set; }

        [ForeignKey("CategoryId"), Required]
        public Category Category { get; set; }
    }
}
