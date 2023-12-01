using Bookshelf_TL.Models;
using Bookshelf_TL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf_SL.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly BookshelfDbContext _dbContext;
        public CategoryRepository(BookshelfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Category> All => _dbContext.Categories.ToList();
        public Category FindById(string id)
        {
            Category category = _dbContext.Categories.FirstOrDefault(e => e.Id == id);

            if (category != null)
            {
                // Завантажуємо BookCategories разом з категоріями
                category.BookCategories = _dbContext.BookCategories.Include(bc => bc.Book).Where(bc => bc.CategoryId == id).ToList();
            }

            return category;
        }
        public void Create(Category entity)
        {
            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(Category entity)
        {
            _dbContext.Categories.Update(entity);
            _dbContext.SaveChanges();
        }
        public void Delete(string id)
        {
            _dbContext.Categories.Remove(FindById(id));
            _dbContext.SaveChanges();
        }
    }
}
