using Bookshelf_TL.Models.IntermediateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf_SL.Repositories.IntermediateModelsRepositories
{
    public interface IIntermediateRepository<IEntity> where IEntity : class
    {
        abstract IEnumerable<IEntity> All { get; }
        abstract IEntity FindById(string firstId, string secondId);
        abstract void Create(IEntity entity);
        public void CreateCollection(IEnumerable<IIntermediateEntity> intermediateEntities);
        abstract void Edit(IEntity entity);
        abstract void Delete(IEntity entity);
        public void DeleteCollection(IEnumerable<IIntermediateEntity> intermediateEntities);
    }
}
