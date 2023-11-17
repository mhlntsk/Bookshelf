using Microsoft.EntityFrameworkCore;
using Bookshelf_TL;
using Bookshelf_TL.Models.IntermediateModels;

namespace Bookshelf_SL.Repositories.IntermediateModelsRepositories
{
    public class BookAuthorRepository : IIntermediateRepository<BookAuthor>
    {
        private readonly BookshelfDbContext _dbContext;
        public BookAuthorRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BookAuthor> All => _dbContext.BookAuthors.Include(ba => ba.Author).Include(ba => ba.Book).ToList();
        public BookAuthor FindById(string bookId, string authorId)
        {
            return _dbContext.BookAuthors.SingleOrDefault(ba => ba.BookId == bookId && ba.AuthorId == authorId);
        }
        public void Create(BookAuthor bookAuthor)
        {
            _dbContext.Add(bookAuthor);
            _dbContext.SaveChanges();
        }
        public void CreateCollection(IEnumerable<IIntermediateEntity> bookAuthors)
        {
            _dbContext.AddRange(bookAuthors);
            _dbContext.SaveChanges();
        }
        public void Edit(BookAuthor bookAuthor)
        {
            _dbContext.Update(bookAuthor);
            _dbContext.SaveChanges();
        }
        public void Delete(BookAuthor bookAuthor)
        {
            _dbContext.Remove(bookAuthor);
            _dbContext.SaveChanges();
        }
        public void DeleteCollection(IEnumerable<IIntermediateEntity> bookAuthors)
        {
            _dbContext.RemoveRange(bookAuthors);
            _dbContext.SaveChanges();
        }
    }
}
