using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_SL.Repositories;
using Bookshelf_TL.Models.IntermediateModels;
using Bookshelf_TL.Models;
using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_FL.Models.BookViewModels;
using Bookshelf_FL.Models;

namespace Bookshelf_FL.Extensions.Services
{
    public class FactoryOfAuthorService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IIntermediateRepository<BookAuthor> _bookAuthorRepository;
        private readonly CoverImageService _coverImageService;
        public FactoryOfAuthorService(
            IRepository<Book> bookRepository,
            IIntermediateRepository<BookAuthor> bookAuthorRepository,
            CoverImageService coverImageService)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _coverImageService = coverImageService;
        }
        public AuthorCreateViewModel AuthorCreateViewModel()
        {
            var authorCreateViewModel = new AuthorCreateViewModel()
            {
                Books = new List<BookDtoViewModel>(),
                SelectedBooks = new List<string>()
            };

            #region Books

            var Books = _bookRepository.All
                .OrderBy(a => a.Series)
                .ThenBy(a => a.NumberInSeries)
                .ThenBy(a => a.BookName)
                .ToList();

            if (Books != null && Books.Any())
            {
                foreach (var book in Books)
                {
                    var bookNameViewModel = new BookDtoViewModel()
                    {
                        Id = book.Id,
                        BookName = book.BookName
                    };

                    authorCreateViewModel.Books.Add(bookNameViewModel);
                }
            }
            #endregion

            return authorCreateViewModel;
        }
        public AuthorEditViewModel AuthorEditViewModel(Author author)
        {
            var authorEditViewModel = new AuthorEditViewModel()
            {
                FirstName = author.FirstName,
                MiddleName = author.MiddleName,
                LastName = author.LastName,
                Country = author.Country,
                Description = author.Description,

                Books = new List<BookDtoViewModel>(),
                SelectedBooks = new List<string>()
            };

            _coverImageService.FillCoverImageIntoViewModels(authorEditViewModel, author);

            #region Fill Books

            var Books = _bookRepository.All
                .OrderBy(a => a.Series)
                .ThenBy(a => a.NumberInSeries)
                .ThenBy(a => a.BookName)
                .ToList(); ;

            if (Books != null && Books.Any())
            {
                foreach (var book in Books)
                {
                    var bookNameViewModel = new BookDtoViewModel()
                    {
                        Id = book.Id,
                        BookName = book.BookName
                    };

                    authorEditViewModel.Books.Add(bookNameViewModel);
                }
            }

            #endregion

            #region Fill SelectedBooks

            var bookAuthors = _bookAuthorRepository.All.Where(ba => ba.AuthorId == author.Id).ToList();

            foreach (var bookAuthor in bookAuthors)
            {
                authorEditViewModel.SelectedBooks.Add(bookAuthor.BookId);
            }

            #endregion

            return authorEditViewModel;
        }
        public IEnumerable<AuthorListViewModel> AuthorListViewModel(IEnumerable<Author> authors)
        {
            var authorsListViewModels = new List<AuthorListViewModel>();

            foreach (var author in authors)
            {
                var authorsListViewModel = new AuthorListViewModel
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    MiddleName = author.MiddleName,
                    Country = author.Country,
                };

                _coverImageService.FillCoverImageIntoViewModels(authorsListViewModel, author);

                authorsListViewModels.Add(authorsListViewModel);
            }

            return authorsListViewModels;
        }
        public AuthorViewModel AuthorViewModel(Author author)
        {
            var books = _bookAuthorRepository.All
                .Where(ba => ba.AuthorId == author.Id)
                .Select(ba => ba.Book)
                .ToList();

            var authorViewModel = new AuthorViewModel()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                MiddleName = author.MiddleName,
                LastName = author.LastName,
                Country = author.Country,
                Description = author.Description,

                SelectedBooks = new List<BookListViewModel>(),
            };

            _coverImageService.FillCoverImageIntoViewModels(authorViewModel, author);

            #region Fill SelectedBooks

            foreach (var book in books)
            {
                var bookListViewModel = new BookListViewModel()
                {
                    Id = book.Id,
                    BookName = book.BookName,
                    Series = book.Series,
                    NumberInSeries = book.NumberInSeries,
                    AverageBookScore = book.AverageBookScore,
                };

                _coverImageService.FillCoverImageIntoViewModels(bookListViewModel, book);

                authorViewModel.SelectedBooks.Add(bookListViewModel);
            }

            #endregion


            return authorViewModel;
        }
    }
}
