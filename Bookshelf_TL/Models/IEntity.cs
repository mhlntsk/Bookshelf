using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf_TL.Models
{
    public interface IEntity
    {
        public string Id { get; set; }
        public string CoverPath { get; set; }
        public string GetDefaultCover();
    }
}
