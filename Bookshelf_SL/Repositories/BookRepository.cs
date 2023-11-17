using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookshelf_TL;
using Bookshelf_TL.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf_SL.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly BookshelfDbContext _dbContext;
        public BookRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Book> All => _dbContext.Books.ToList();
        public Book FindById(string id)
        {
            Book book = _dbContext.Books.FirstOrDefault(e => e.Id == id);

            if (book != null)
            {
                // Завантажуємо BookCategories, BookAuthors, BookUsers разом з книгою
                book.BookCategories = _dbContext.BookCategories.Include(bc => bc.Category).Where(bc => bc.BookId == id).ToList();
                book.BookAuthors = _dbContext.BookAuthors.Include(ba => ba.Author).Where(ba => ba.BookId == id).ToList();
                book.BookUsers = _dbContext.BookUsers.Include(bu => bu.User).Where(bu => bu.BookId == id).ToList();
            }

            return book;
        }
        public void Create(Book entity)
        {
            _dbContext.Books.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(Book entity)
        {
            _dbContext.Books.Update(entity);
            _dbContext.SaveChanges();
        }
        public void Delete(string id)
        {
            _dbContext.Books.Remove(FindById(id));
            _dbContext.SaveChanges();
        }
    }
}
