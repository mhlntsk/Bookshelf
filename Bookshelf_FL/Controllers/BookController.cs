using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.Models.BookViewModels;
using Bookshelf_SL.Repositories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookshelf_FL.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly UserManager<User> _userManager;
        private readonly CoverImageService _coverImageService;
        private readonly RelationEntitiesService _relationEntitiesService;
        private readonly FactoryOfBookService _factoryOfBookService;
        public BookController(
            IRepository<Book> bookRepository,
            UserManager<User> userManager,
            CoverImageService coverImageService,
            RelationEntitiesService relationEntitiesService,
            FactoryOfBookService factoryOfBookService)
        {
            _bookRepository = bookRepository;
            _userManager = userManager;
            _coverImageService = coverImageService;
            _relationEntitiesService = relationEntitiesService;
            _factoryOfBookService = factoryOfBookService;
        }

        public async Task<IActionResult> GetBooks() 
        {
            var books = _bookRepository.All;

            var currentUser = await _userManager.GetUserAsync(User);

            var bookListViewModels = _factoryOfBookService.BookListViewModels(currentUser, books);

            return View(bookListViewModels);
        }
        public async Task<IActionResult> SearchBook(string SearchQuery) 
        {
            IEnumerable<Book> filteredBooks;

            if (SearchQuery == null)
            {
                filteredBooks = _bookRepository.All;
            }
            else
            {
                filteredBooks = _bookRepository.All.Where(book => (book.BookName != null && book.BookName.Contains(SearchQuery)));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var bookListViewModels = _factoryOfBookService.BookListViewModels(currentUser, filteredBooks);

            return View(bookListViewModels);
        }
        public async Task<IActionResult> GetBook(string bookId) 
        {
            var book = _bookRepository.FindById(bookId);

            var currentUser = await _userManager.GetUserAsync(User);

            var bookViewModel = _factoryOfBookService.BookViewModel(book, currentUser);

            return View(bookViewModel);
        }
        public IActionResult Create() 
        {
            var bookCreateViewModel = _factoryOfBookService.BookCreateViewModel();

            return View(bookCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateViewModel bookCreateViewModel)
        {
            Book book = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = bookCreateViewModel.BookName,
                DateOfPublication = bookCreateViewModel.DateOfPublication,
                Series = bookCreateViewModel.Series,
                NumberInSeries = bookCreateViewModel.NumberInSeries,
                Description = bookCreateViewModel.Description,
                AverageBookScore = 0,
            };

            if (bookCreateViewModel.CoverImage != null && bookCreateViewModel.CoverImage.Length > 0)
            {
                string folderToSaveName = "BookCovers";
                book.CoverPath = await _coverImageService.AddCover(bookCreateViewModel.CoverImage, folderToSaveName);
            }

            _bookRepository.Create(book);

            if (bookCreateViewModel.SelectedAuthors != null && bookCreateViewModel.SelectedAuthors.Any())
            {
                _relationEntitiesService.UpdateBookAuthorsInsideBook(book.Id, bookCreateViewModel.SelectedAuthors);
            }

            if (bookCreateViewModel.SelectedCategories != null && bookCreateViewModel.SelectedCategories.Any())
            {
                _relationEntitiesService.UpdateBookCategoriesInsideBook(book.Id, bookCreateViewModel.SelectedCategories);
            }

            return RedirectToAction("GetBook", new { bookId = book.Id });
        }
        public IActionResult Edit(string id) 
        {
            var book = _bookRepository.FindById(id);

            if (book == null)
                return NotFound();

            var bookEditViewModel = _factoryOfBookService.BookEditViewModel(book);

            return View(bookEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookEditViewModel bookEditViewModel)
        {
            var book = _bookRepository.FindById(bookEditViewModel.Id);

            book.BookName = bookEditViewModel.BookName != null ? bookEditViewModel.BookName : book.BookName;
            book.DateOfPublication = bookEditViewModel.DateOfPublication;
            book.Series = bookEditViewModel.Series;
            book.NumberInSeries = bookEditViewModel.NumberInSeries;
            book.Description = bookEditViewModel.Description;

            if (bookEditViewModel.SelectedAuthors != null && bookEditViewModel.SelectedAuthors.Any())
            {
                _relationEntitiesService.UpdateBookAuthorsInsideBook(book.Id, bookEditViewModel.SelectedAuthors);
            }

            if (bookEditViewModel.CoverImage != null && bookEditViewModel.CoverImage.Length > 0)
            {
                string folderToSaveName = "BookCovers";
                book.CoverPath = await _coverImageService.AddCover(bookEditViewModel.CoverImage, folderToSaveName);
            }

            _bookRepository.Update(book);

            return RedirectToAction("GetBook", new { bookId = book.Id });
        }

        [HttpPost]
        public IActionResult DeleteImage(string id) 
        {
            var book = _bookRepository.FindById(id);

            _coverImageService.DeleteCover(_bookRepository, book);

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        public IActionResult Delete(
            string bookId,
            [FromServices] IIntermediateRepository<BookUser> bookUserRepository,
            [FromServices] IIntermediateRepository<BookAuthor> bookAuthorRepository,
            [FromServices] IIntermediateRepository<BookCategory> bookCategoryRepository
            )
        {
            var book = _bookRepository.FindById(bookId);

            var bookUsers = bookUserRepository.All.Where(ba => ba.BookId == book.Id);
            var bookAuthors = bookAuthorRepository.All.Where(ba => ba.BookId == book.Id);
            var bookCategories = bookCategoryRepository.All.Where(ba => ba.BookId == book.Id);

            if (book == null)
                return NotFound();

            _coverImageService.DeleteCover(_bookRepository, book);

            bookUserRepository.DeleteCollection(bookUsers);
            bookAuthorRepository.DeleteCollection(bookAuthors);
            bookCategoryRepository.DeleteCollection(bookCategories);

            _bookRepository.Delete(book.Id);

            return RedirectToAction("GetBooks");
        }

    }
}
