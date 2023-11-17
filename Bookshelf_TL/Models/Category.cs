using Bookshelf_TL.Models.IntermediateModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookshelf_TL.Models
{
    public class Category
    {
        [Required, Key]
        public string Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Category")]
        public ICollection<BookCategory> BookCategories { get; set; }
    }
}
