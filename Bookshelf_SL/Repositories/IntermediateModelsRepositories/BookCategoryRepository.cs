using Bookshelf_TL;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf_SL.Repositories.IntermediateModelsRepositories
{
    public class BookCategoryRepository : IIntermediateRepository<BookCategory>
    {
        private readonly BookshelfDbContext _dbContext;
        public BookCategoryRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BookCategory> All => _dbContext.BookCategories.Include(ba => ba.Category).Include(ba => ba.Book).ToList();
        public BookCategory FindById(string bookId, string categoryId)
        {
            return _dbContext.BookCategories.SingleOrDefault(bc => bc.BookId == bookId && bc.CategoryId == categoryId);
        }
        public void Create(BookCategory bookCategory)
        {
            _dbContext.Add(bookCategory);
            _dbContext.SaveChanges();
        }
        public void CreateCollection(IEnumerable<IIntermediateEntity> bookCategories)
        {
            _dbContext.AddRange(bookCategories);
            _dbContext.SaveChanges();
        }
        public void Edit(BookCategory bookCategory)
        {
            _dbContext.Update(bookCategory);
            _dbContext.SaveChanges();
        }
        public void Delete(BookCategory bookCategory)
        {
            _dbContext.Remove(bookCategory);
            _dbContext.SaveChanges();
        }
        public void DeleteCollection(IEnumerable<IIntermediateEntity> bookCategories)
        {
            _dbContext.RemoveRange(bookCategories);
            _dbContext.SaveChanges();
        }
    }
}
