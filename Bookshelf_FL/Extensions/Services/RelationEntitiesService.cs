using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bookshelf_FL.Extensions.Services
{
    public class RelationEntitiesService
    {
        private readonly IIntermediateRepository<BookAuthor> _bookAuthorRepository;
        private readonly IIntermediateRepository<BookCategory> _bookCategoryRepository;
        private readonly IIntermediateRepository<BookUser> _bookUserRepository;

        public RelationEntitiesService(
            IIntermediateRepository<BookAuthor> bookAuthorRepository, 
            IIntermediateRepository<BookCategory> bookCategoryRepository,
            IIntermediateRepository<BookUser> bookUserRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
            _bookCategoryRepository = bookCategoryRepository;
            _bookUserRepository = bookUserRepository;
        }

        public void UpdateBookAuthorsInsideAuthor(string authorId, List<string> bookIds)
        {
            // 1. Отримали усі BookAuthor для конкретного автора
            ICollection<BookAuthor> originalBookAuthors = _bookAuthorRepository.All.Where(ba => ba.AuthorId == authorId).ToList();

            // 2. Отримали список ідентифікаторів книг для оригінального запису
            var originalBookIds = originalBookAuthors.Select(ba => ba.BookId).ToList();

            // 3. Отримали список книг, які були видалені користувачем
            var bookIdsToDelete = originalBookIds.Except(bookIds);

            // 4. Видалили записи з БД, які були видалені користувачем
            foreach (var bookIdToDelete in bookIdsToDelete)
            {
                var bookAuthorToDelete = originalBookAuthors
                    .SingleOrDefault(ba => ba.AuthorId == authorId && ba.BookId == bookIdToDelete);

                if (bookAuthorToDelete != null)
                {
                    _bookAuthorRepository.Delete(bookAuthorToDelete);
                }
            }

            // 5. Перевірити, які нові книги були додані користувачем
            var bookIdsToAdd = bookIds.Except(originalBookIds);

            // 6. Додати нові записи
            if (bookIdsToAdd != null)
            {
                foreach (var bookIdToAdd in bookIdsToAdd)
                {
                    var newBookAuthor = new BookAuthor
                    {
                        AuthorId = authorId,
                        BookId = bookIdToAdd
                    };

                    _bookAuthorRepository.Create(newBookAuthor);
                }
            }
        }
        public void UpdateBookAuthorsInsideBook(string bookId, List<string> authorIds)
        {
            var originalBookAuthors = _bookAuthorRepository.All.Where(ba => ba.BookId == bookId).ToList();

            var originalAuthorIds = originalBookAuthors.Select(ba => ba.AuthorId).ToList();

            var authorIdsToDelete = originalAuthorIds.Except(authorIds);

            foreach (var authorIdToDelete in authorIdsToDelete)
            {
                var bookAuthorToDelete = originalBookAuthors
                    .SingleOrDefault(ba => ba.BookId == bookId && ba.AuthorId == authorIdToDelete);

                if (bookAuthorToDelete != null)
                {
                    _bookAuthorRepository.Delete(bookAuthorToDelete);
                }
            }

            var authorIdsToAdd = authorIds.Except(originalAuthorIds);

            if (authorIdsToAdd != null && authorIdsToAdd.Any())
            {
                foreach (var authorIdToAdd in authorIdsToAdd)
                {
                    var newBookAuthor = new BookAuthor
                    {
                        AuthorId = authorIdToAdd,
                        BookId = bookId
                    };

                    _bookAuthorRepository.Create(newBookAuthor);
                }
            }
        }
        public void UpdateBookCategoriesInsideBook(string bookId, List<string> categoryIds)
        {
            var originalBookCategories = _bookCategoryRepository.All.Where(ba => ba.BookId == bookId).ToList();

            var originalCategoryIds = originalBookCategories.Select(ba => ba.CategoryId).ToList();

            var categoriesToDelete = originalCategoryIds.Except(categoryIds);

            foreach (var categoryIdToDelete in categoriesToDelete)
            {
                var bookCategoriesToDelete = originalBookCategories
                    .SingleOrDefault(ba => ba.BookId == bookId && ba.CategoryId == categoryIdToDelete);

                if (bookCategoriesToDelete != null)
                {
                    _bookCategoryRepository.Delete(bookCategoriesToDelete);
                }
            }

            var categoriesToAdd = categoryIds.Except(originalCategoryIds);

            if (categoriesToAdd != null)
            {
                foreach (var categoryIdToAdd in categoriesToAdd)
                {
                    var newBookCategory = new BookCategory
                    {
                        BookId = bookId,
                        CategoryId = categoryIdToAdd
                    };

                    _bookCategoryRepository.Create(newBookCategory);
                }
            }
        }
        public void UpdateBookCategoriesInsideCategory(string categoryId, List<string> bookIds)
        {
            ICollection<BookCategory> originalBookCategories = _bookCategoryRepository.All.Where(ba => ba.CategoryId == categoryId).ToList();

            var originalBookIds = originalBookCategories.Select(ba => ba.BookId).ToList();

            var booksToDelete = originalBookIds.Except(bookIds);

            foreach (var bookIdToDelete in booksToDelete)
            {
                var bookCategoryToDelete = originalBookCategories
                    .SingleOrDefault(ba => ba.CategoryId == categoryId && ba.BookId == bookIdToDelete);

                if (bookCategoryToDelete != null)
                {
                    _bookCategoryRepository.Delete(bookCategoryToDelete);
                }
            }

            var booksToAdd = bookIds.Except(originalBookIds);

            if (booksToAdd != null)
            {
                foreach (var bookIdToAdd in booksToAdd)
                {
                    var newBookCategory = new BookCategory
                    {
                        BookId = bookIdToAdd,
                        CategoryId = categoryId
                    };

                    _bookCategoryRepository.Create(newBookCategory);
                }
            }
        }
        public void UpdateBookUsersInsideBook(string bookId, List<string> userIds)
        {
            var originalBookUsers = _bookUserRepository.All.Where(ba => ba.BookId == bookId).ToList();

            var originalUserIds = originalBookUsers.Select(ba => ba.UserId).ToList();

            var usersToDelete = originalUserIds.Except(userIds);

            foreach (var userIdToDelete in usersToDelete)
            {
                var bookUserToDelete = originalBookUsers
                    .SingleOrDefault(ba => ba.BookId == bookId && ba.UserId == userIdToDelete);

                if (bookUserToDelete != null)
                {
                    _bookUserRepository.Delete(bookUserToDelete);
                }
            }

            var usersToAdd = userIds.Except(originalUserIds);

            if (usersToAdd != null)
            {
                foreach (var userIdToAdd in usersToAdd)
                {
                    var newBookUser = new BookUser
                    {
                        BookId = bookId,
                        UserId = userIdToAdd
                    };

                    _bookUserRepository.Create(newBookUser);
                }
            }
        }
        public void UpdateBookUsersInsideUser(string userId, List<string> bookIds)
        {
            var originalBookUsers = _bookUserRepository.All.Where(ba => ba.UserId == userId).ToList();

            var originalBookIds = originalBookUsers.Select(ba => ba.BookId).ToList();

            var bookIdsToDelete = originalBookIds.Except(bookIds);

            foreach (var bookIdToDelete in bookIdsToDelete)
            {
                var bookUserToDelete = originalBookUsers
                    .SingleOrDefault(ba => ba.UserId == userId && ba.BookId == bookIdToDelete);

                if (bookUserToDelete != null)
                {
                    _bookUserRepository.Delete(bookUserToDelete);
                }
            }

            var bookIdsToAdd = bookIds.Except(originalBookIds);

            if (bookIdsToAdd != null)
            {
                foreach (var bookIdToAdd in bookIdsToAdd)
                {
                    var newBookUser = new BookUser
                    {
                        BookId = bookIdToAdd,
                        UserId = userId
                    };

                    _bookUserRepository.Create(newBookUser);
                }
            }
        }
    }
}
