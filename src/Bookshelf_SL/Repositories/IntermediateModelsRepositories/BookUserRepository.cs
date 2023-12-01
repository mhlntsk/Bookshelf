using Bookshelf_TL.Models.IntermediateModels;
using Bookshelf_TL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Bookshelf_TL.Models;

namespace Bookshelf_SL.Repositories.IntermediateModelsRepositories
{
    public class BookUserRepository : IIntermediateRepository<BookUser>
    {
        private readonly BookshelfDbContext _dbContext;
        public BookUserRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BookUser> All => _dbContext.BookUsers.Include(ba => ba.User).Include(ba => ba.Book).ToList();
        public BookUser FindById(string bookId, string userId)
        {
            return _dbContext.BookUsers.SingleOrDefault(bc => bc.BookId == bookId && bc.UserId == userId);
        }
        public void Create(BookUser bookUser)
        {
            _dbContext.Add(bookUser);
            _dbContext.SaveChanges();
        }
        public void CreateCollection(IEnumerable<IIntermediateEntity> bookUsers)
        {
            _dbContext.AddRange(bookUsers);
            _dbContext.SaveChanges();
        }
        public void Edit(BookUser bookUser)
        {
            _dbContext.Update(bookUser);
            _dbContext.SaveChanges();
        }
        public void Delete(BookUser bookUser)
        {
            _dbContext.Remove(bookUser);
            _dbContext.SaveChanges();
        }
        public void DeleteCollection(IEnumerable<IIntermediateEntity> bookUsers)
        {
            _dbContext.RemoveRange(bookUsers);
            _dbContext.SaveChanges();
        }
    }
}
