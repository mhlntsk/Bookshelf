using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.BookViewModels;
using Bookshelf_FL.Models.Categories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models.IntermediateModels;
using Bookshelf_TL.Models;
using Bookshelf_SL.Repositories;
using Bookshelf_FL.Models;

namespace Bookshelf_FL.Extensions.Services
{
    public class FactoryOfBookService
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IIntermediateRepository<BookUser> _bookUserRepository;
        private readonly IIntermediateRepository<BookAuthor> _bookAuthorRepository;
        private readonly IIntermediateRepository<BookCategory> _bookCategoryRepository;
        private readonly CoverImageService _coverImageService;
        public FactoryOfBookService(
            IRepository<Author> authorRepository,
            IRepository<Category> categoryRepository,
            IIntermediateRepository<BookUser> bookUserRepository,
            IIntermediateRepository<BookAuthor> bookAuthorRepository,
            IIntermediateRepository<BookCategory> bookCategoryRepository,
            CoverImageService coverImageService)
        {
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _bookUserRepository = bookUserRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookCategoryRepository = bookCategoryRepository;
            _coverImageService = coverImageService;
        }

        public BookCreateViewModel BookCreateViewModel()
        {
            var bookCreateViewModel = new BookCreateViewModel()
            {
                Categories = new List<CategoryDtoViewModel>(),
                Authors = new List<AuthorDtoViewModel>(),
            };

            #region Fill authors of book

            var allAuthors = _authorRepository.All
                .OrderBy(a => a.FirstName)
                .ToList();

            if (allAuthors != null && allAuthors.Any())
            {
                foreach (var author in allAuthors)
                {
                    var authorLiteViewModel = new AuthorDtoViewModel()
                    {
                        Id = author.Id,
                        AuthorFirstName = author.FirstName,
                        AuthorLastName = author.LastName,
                    };

                    bookCreateViewModel.Authors.Add(authorLiteViewModel);
                }
            }

            #endregion

            #region Fill categories of book

            var allCategories = _categoryRepository.All
                .OrderBy(a => a.Name)
                .ToList();

            if (allCategories != null && allCategories.Any())
            {
                foreach (var category in allCategories)
                {
                    var categoryLiteViewModel = new CategoryDtoViewModel()
                    {
                        Id = category.Id,
                        CategoryName = category.Name,
                    };

                    bookCreateViewModel.Categories.Add(categoryLiteViewModel);
                }
            }

            #endregion

            return bookCreateViewModel;
        }
        public BookEditViewModel BookEditViewModel(Book book)
        {
            var bookEditViewModel = new BookEditViewModel
            {
                Id = book.Id,
                BookName = book.BookName,
                DateOfPublication = book.DateOfPublication,
                Series = book.Series,
                NumberInSeries = book.NumberInSeries,
                Description = book.Description,

                Categories = new List<CategoryDtoViewModel>(),
                Authors = new List<AuthorDtoViewModel>(),
                SelectedCategories = new List<string>(),
                SelectedAuthors = new List<string>()
            };

            _coverImageService.FillCoverImageIntoViewModels(bookEditViewModel, book);

            #region Fill authors

            var authors = _authorRepository.All
                .OrderBy(a => a.FirstName)
                .ToList(); ;

            foreach (var author in authors)
            {
                var authorNameViewModel = new AuthorDtoViewModel()
                {
                    Id = author.Id,
                    AuthorFirstName = author.FirstName,
                    AuthorLastName = author.LastName,
                };

                bookEditViewModel.Authors.Add(authorNameViewModel);
            }

            #endregion

            #region Fill categories

            var categories = _categoryRepository.All
                .OrderBy(a => a.Name)
                .ToList(); ;

            foreach (var category in categories)
            {
                var categoryNameViewModel = new CategoryDtoViewModel()
                {
                    Id = category.Id,
                    CategoryName = category.Name,
                };

                bookEditViewModel.Categories.Add(categoryNameViewModel);
            }

            #endregion

            #region Fill SelectedAuthors

            var bookAuthors = _bookAuthorRepository.All
                .Where(ba => ba.BookId == book.Id)
                .ToList();

            foreach (var bookAuthor in bookAuthors)
            {
                bookEditViewModel.SelectedAuthors.Add(bookAuthor.AuthorId);
            }

            #endregion

            #region Fill SelectedCategories

            var bookCategories = _bookCategoryRepository.All
                .Where(ba => ba.BookId == book.Id)
                .ToList();

            foreach (var bookCategory in bookCategories)
            {
                bookEditViewModel.SelectedCategories.Add(bookCategory.CategoryId);
            }

            #endregion

            return bookEditViewModel;
        }
        public IEnumerable<BookListViewModel> BookListViewModels(User currentUser, IEnumerable<Book> books)
        {
            var bookListOfUserViewModels = new List<BookListViewModel>();

            var selectedBookUsers = _bookUserRepository.All.Where(b => b.UserId == currentUser.Id);

            var bookCategories = _bookCategoryRepository.All.Where(bc => books.Any(book => book.Id == bc.BookId));
            var bookAuthors = _bookAuthorRepository.All.Where(ba => books.Any(book => book.Id == ba.BookId));

            foreach (var book in books)
            {
                var bookListOfUserViewModel = new BookListViewModel
                {
                    Id = book.Id,
                    BookName = book.BookName,
                    Series = book.Series,
                    NumberInSeries = book.NumberInSeries,
                    AverageBookScore = book.AverageBookScore,

                    Categories = new List<CategoryDtoViewModel>(),
                    Authors = new List<AuthorDtoViewModel>(),
                };

                _coverImageService.FillCoverImageIntoViewModels(bookListOfUserViewModel, book);

                #region Fill StatusOfCurrentUser

                if (selectedBookUsers != null)
                {
                    var selectedBookUser = selectedBookUsers.FirstOrDefault(bu => bu.BookId == book.Id) as BookUser;

                    if (selectedBookUser != null)
                    {
                        bookListOfUserViewModel.StatusOfUser = selectedBookUser.Status == null ? "" : selectedBookUser.Status;
                    }
                }

                #endregion

                #region Fill Categories

                foreach (var bookCategory in bookCategories)
                {
                    if (bookCategory.BookId == book.Id)
                    {
                        var categoryNameViewModel = new CategoryDtoViewModel()
                        {
                            CategoryName = bookCategory.Category.Name,
                            Id = bookCategory.Category.Id
                        };

                        bookListOfUserViewModel.Categories.Add(categoryNameViewModel);
                    }
                }

                #endregion

                #region Fill Authors

                foreach (var bookAuthor in bookAuthors)
                {
                    if (bookAuthor.BookId == book.Id)
                    {
                        var authorNameViewModel = new AuthorDtoViewModel()
                        {
                            Id = bookAuthor.Author.Id,
                            AuthorFirstName = bookAuthor.Author.FirstName,
                            AuthorLastName = bookAuthor.Author.LastName,
                        };

                        bookListOfUserViewModel.Authors.Add(authorNameViewModel);
                    }
                }

                #endregion

                bookListOfUserViewModels.Add(bookListOfUserViewModel);
            }

            return bookListOfUserViewModels;
        }
        public BookViewModel BookViewModel(Book book, User user)
        {
            var bookUser = _bookUserRepository.All
                .Where(a => a.UserId == user.Id)
                .FirstOrDefault(bu => bu.BookId == book.Id);

            var authors = _bookAuthorRepository.All
                .Where(ba => ba.BookId == book.Id)
                .Select(ba => ba.Author)
                .ToList();

            var categories = _bookCategoryRepository.All
                .Where(bc => bc.BookId == book.Id)
                .Select(bc => bc.Category)
                .ToList();

            var bookViewModel = new BookViewModel()
            {
                Id = book.Id,
                BookName = book.BookName,
                DateOfPublication = book.DateOfPublication,
                Series = book.Series,
                NumberInSeries = book.NumberInSeries,
                Description = book.Description,
                AverageBookScore = book.AverageBookScore,

                Authors = new List<AuthorDtoViewModel>(),
                Categories = new List<CategoryDtoViewModel>(),
            };

            _coverImageService.FillCoverImageIntoViewModels(bookViewModel, book);

            #region Fill StatusOfCurrentUser & IndividualBookScore

            if (bookUser != null)
            {
                bookViewModel.StatusOfCurrentUser = bookUser.Status;
                bookViewModel.IndividualBookScore = bookUser.IndividualBookScore;
            }

            #endregion

            #region Fill authors of book

            foreach (var author in authors)
            {
                var authorNameViewModel = new AuthorDtoViewModel()
                {
                    Id = author.Id,
                    AuthorFirstName = author.FirstName,
                    AuthorLastName = author.LastName,
                };

                bookViewModel.Authors.Add(authorNameViewModel);
            }

            #endregion

            #region Fill categories of book

            foreach (var category in categories)
            {
                var categoryNameViewModel = new CategoryDtoViewModel()
                {
                    Id = category.Id,
                    CategoryName = category.Name,
                };

                bookViewModel.Categories.Add(categoryNameViewModel);
            }

            #endregion

            return bookViewModel;
        }
    }
}
