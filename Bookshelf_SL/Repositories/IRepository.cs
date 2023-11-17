using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf_SL.Repositories
{
    public interface IRepository<IEntity> where IEntity : class
    {
        abstract ICollection<IEntity> All { get; }
        abstract IEntity FindById(string id);
        abstract void Create(IEntity entity);
        abstract void Update(IEntity entity);
        abstract void Delete(string id);
    }
}
