using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.BookViewModels;
using Bookshelf_FL.Models.Categories;
using Bookshelf_FL.Models.UserViewModels;
using Bookshelf_SL.Repositories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf_FL.Extensions.Services
{
    public class BookScoreService
    {
        private IRepository<Book> _bookRepository;
        private IIntermediateRepository<BookUser> _bookUserRepository;

        public BookScoreService(
            IRepository<Book> bookRepository, 
            IIntermediateRepository<BookUser> bookUserRepository)
        {
            _bookUserRepository = bookUserRepository;
            _bookRepository = bookRepository;
        }
        
        




        public void UpdateAverageBookScore(string bookId)
        {
            var book = _bookRepository.FindById(bookId);
            if (book == null)
            {
                throw new NullReferenceException();
            }

            var bookUsersOfBook = _bookUserRepository.All.Where(bu => bu.BookId == bookId);
            if (bookUsersOfBook.Count() == 0 || bookUsersOfBook == null)
            {
                book.AverageBookScore = 0;
            }
            else
            {
                var nonNullIndividualScores = bookUsersOfBook
                    .Where(bu => bu.IndividualBookScore != null)
                    .Select(bu => bu.IndividualBookScore.Value);

                decimal averageScore = nonNullIndividualScores.Any() ? (decimal)nonNullIndividualScores.Average() : 0;

                book.AverageBookScore = averageScore;
            }

            _bookRepository.Update(book);
        }
    }
}
