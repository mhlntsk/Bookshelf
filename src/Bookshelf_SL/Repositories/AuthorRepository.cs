using Bookshelf_TL;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf_SL.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly BookshelfDbContext _dbContext;
        public AuthorRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Author> All => _dbContext.Authors.ToList();
        public Author FindById(string id)
        {
            Author author = _dbContext.Authors.FirstOrDefault(e => e.Id == id);

            if (author != null)
            {
                // Завантажуємо BookAuthors разом з авторами
                author.BookAuthors = _dbContext.BookAuthors.Include(ba => ba.Book).Where(ba => ba.AuthorId == id).ToList();
            }

            return author;
        }
        public void Create(Author entity)
        {
            _dbContext.Authors.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(Author entity)
        {
            _dbContext.Authors.Update(entity);
            _dbContext.SaveChanges();
        }
        public void Delete(string id)
        {
            _dbContext.Authors.Remove(FindById(id));
            _dbContext.SaveChanges();
        }
    }
}
