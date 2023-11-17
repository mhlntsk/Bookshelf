using Bookshelf_TL;
using Bookshelf_TL.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf_SL.Repositories
{
    public class UserRepository: IRepository<User>
    {
        private readonly BookshelfDbContext _dbContext;
        public UserRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<User> All => _dbContext.Users.ToList();
        public User FindById(string stringId)
        {
            User user = _dbContext.Users.FirstOrDefault(e => e.Id == stringId);

            if (user != null)
            {
                // Завантажуємо UserBooks разом з користувачем
                user.BookUsers = _dbContext.BookUsers.Include(ub => ub.Book).Where(ub => ub.UserId == stringId).ToList();
            }

            return user;
        }
        public void Create(User entity)
        {
            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(User entity)
        {
            _dbContext.Users.Update(entity);
            _dbContext.SaveChanges();
        }
        public void Delete(string id)
        {
            _dbContext.Users.Remove(FindById(id));
            _dbContext.SaveChanges();
        }
    }
}
