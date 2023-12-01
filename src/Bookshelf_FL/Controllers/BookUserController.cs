using Bookshelf_FL.Extensions.Services;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bookshelf_FL.Controllers
{
    public class BookUserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IIntermediateRepository<BookUser> _bookUserRepository;
        private readonly BookScoreService _bookService;
        public BookUserController(
            UserManager<User> userManager,
            IIntermediateRepository<BookUser> bookUserRepository, 
            BookScoreService bookService)
        {
            _userManager = userManager;
            _bookUserRepository = bookUserRepository;
            _bookService = bookService;
        }
        public async Task<IActionResult> SetStatus(string bookId, string status)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var bookUser = _bookUserRepository.FindById(bookId, currentUser.Id);

            if (bookUser == null)
            {
                bookUser = new BookUser()
                {
                    BookId = bookId,
                    UserId = currentUser.Id,
                    Status = status
                };

                _bookUserRepository.Create(bookUser);
            }
            else
            {
                if (string.Equals(bookUser.Status, status))
                {
                    bookUser.Status = "";
                }
                else
                {
                    bookUser.Status = status;
                }

                _bookUserRepository.Edit(bookUser);
            }

            var returnUrl = HttpContext.Request.Headers["Referer"].ToString();

            return Redirect(returnUrl);
        }
        public async Task<IActionResult> SetRating(string bookId, int rating, string StatusOfCurrentUser)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var bookUser = _bookUserRepository.FindById(bookId, currentUser.Id);

            if (bookUser == null)
            {
                bookUser = new BookUser()
                {
                    BookId = bookId,
                    UserId = currentUser.Id,
                };

                _bookUserRepository.Create(bookUser);
            }

            if (rating >= 1 && rating <= 5)
            {
                bookUser.IndividualBookScore = rating;

                _bookUserRepository.Edit(bookUser);
            }
            else
            {
                if (StatusOfCurrentUser == null)
                {
                    _bookUserRepository.Delete(bookUser);
                }
                else
                {
                    bookUser.IndividualBookScore = null;

                    _bookUserRepository.Edit(bookUser);
                }
            }

            _bookService.UpdateAverageBookScore(bookId);

            var returnUrl = HttpContext.Request.Headers["Referer"].ToString();

            return Redirect(returnUrl);
        }

        public IActionResult Remove(string bookId, string userId)
        {
            var bookUser = _bookUserRepository.FindById(bookId, userId);

            _bookUserRepository.Delete(bookUser);

            var returnUrl = HttpContext.Request.Headers["Referer"].ToString();

            return Redirect(returnUrl);
        }

    }
}
