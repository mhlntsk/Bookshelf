using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf_TL.Models.IntermediateModels
{
    public class BookUser : IIntermediateEntity
    {
        [Key]
        public string UserId { get; set; }

        [Key]
        public string BookId { get; set; }


        [ForeignKey("UserId"), Required]
        public User User { get; set; }

        [ForeignKey("BookId"), Required]
        public Book Book { get; set; }

        public string Status { get; set; }
        public int? IndividualBookScore { get; set; }
    }
}
