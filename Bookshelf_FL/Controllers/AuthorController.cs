using Microsoft.AspNetCore.Mvc;
using Bookshelf_TL.Models;
using Bookshelf_SL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Bookshelf_FL.Models.BookViewModels;
using Bookshelf_FL.Models.AuthorViewModels;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_FL.Extensions.Services;
using Bookshelf_TL.Models.IntermediateModels;

namespace Bookshelf_FL.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly CoverImageService _coverImageService;
        private readonly RelationEntitiesService _relationEntitiesService;
        private readonly FactoryOfAuthorService _factoryOfAuthorService;
        public AuthorController(
            IRepository<Author> authorRepository,
            CoverImageService coverImageService,
            RelationEntitiesService relationEntitiesService,
            FactoryOfAuthorService factoryOfAuthorService
            )
        {
            _authorRepository = authorRepository;
            _coverImageService = coverImageService;
            _relationEntitiesService = relationEntitiesService;
            _factoryOfAuthorService = factoryOfAuthorService;
        }

        public IActionResult GetAuthors() 
        {
            var authors = _authorRepository.All;

            var authorListViewModels = _factoryOfAuthorService.AuthorListViewModel(authors);

            return View(authorListViewModels);
        }
        public IActionResult SearchAuthor(string SearchQuery) 
        {
            IEnumerable<Author> filteredAuthors;

            if (SearchQuery == null)
            {
                filteredAuthors = _authorRepository.All;
            }
            else
            {
                filteredAuthors = _authorRepository.All
                .Where(author =>
                    (author.FirstName != null && author.FirstName.Contains(SearchQuery))
                    || (author.MiddleName != null && author.MiddleName.Contains(SearchQuery))
                    || (author.LastName != null && author.LastName.Contains(SearchQuery)));
            }

            var authorListViewModels = _factoryOfAuthorService.AuthorListViewModel(filteredAuthors);

            return View(authorListViewModels);
        }
        public IActionResult GetAuthor(string authorId) 
        {
            var author = _authorRepository.FindById(authorId);

            var authorViewModel = _factoryOfAuthorService.AuthorViewModel(author);

            return View(authorViewModel);
        }
        public IActionResult Create()
        {
            var authorCreateViewModel = _factoryOfAuthorService.AuthorCreateViewModel();

            return View(authorCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateViewModel authorCreateViewModel)
        {
            Author author = new Author
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = authorCreateViewModel.FirstName,
                MiddleName = authorCreateViewModel.MiddleName,
                LastName = authorCreateViewModel.LastName,
                Country = authorCreateViewModel.Country,
                Description = authorCreateViewModel.Description,
            };

            if (authorCreateViewModel.CoverImage != null && authorCreateViewModel.CoverImage.Length > 0)
            {
                string folderToSaveName = "AuthorCovers";
                author.CoverPath = await _coverImageService.AddCover(authorCreateViewModel.CoverImage, folderToSaveName);
            }

            _authorRepository.Create(author);

            if (authorCreateViewModel.SelectedBooks != null && authorCreateViewModel.SelectedBooks.Any())
            {
                _relationEntitiesService.UpdateBookAuthorsInsideAuthor(author.Id, authorCreateViewModel.SelectedBooks);
            }

            return RedirectToAction("GetAuthor", new { authorId = author.Id });
        }

        public IActionResult Edit(string id) 
        {
            var author = _authorRepository.FindById(id);

            if (author == null)
                return NotFound();

            var authorEditViewModel = _factoryOfAuthorService.AuthorEditViewModel(author);

            return View(authorEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorEditViewModel authorSetUpViewModel)
        {
            var author = _authorRepository.FindById(authorSetUpViewModel.Id);

            author.FirstName = authorSetUpViewModel.FirstName != null ? authorSetUpViewModel.FirstName : author.FirstName;
            author.MiddleName = authorSetUpViewModel.MiddleName;
            author.LastName = authorSetUpViewModel.LastName != null ? authorSetUpViewModel.LastName : author.LastName;
            author.Country = authorSetUpViewModel.Country;
            author.Description = authorSetUpViewModel.Description;

            if (authorSetUpViewModel.CoverImage != null && authorSetUpViewModel.CoverImage.Length > 0)
            {
                string folderToSaveName = "AuthorCovers";
                author.CoverPath = await _coverImageService.AddCover(authorSetUpViewModel.CoverImage, folderToSaveName);
            }

            _authorRepository.Update(author);

            if (authorSetUpViewModel.SelectedBooks != null && authorSetUpViewModel.SelectedBooks.Any())
            {
                _relationEntitiesService.UpdateBookAuthorsInsideAuthor(author.Id, authorSetUpViewModel.SelectedBooks);
            }

            return RedirectToAction("GetAuthor", new { authorId = author.Id });
        }

        [HttpPost]
        public IActionResult DeleteImage(string id) 
        {
            var author = _authorRepository.FindById(id);

            _coverImageService.DeleteCover(_authorRepository, author);

            return RedirectToAction("Edit", new { userId = id });
        }

        [HttpPost]
        public IActionResult Delete(string authorId, [FromServices] IIntermediateRepository<BookAuthor> bookAuthorRepository) 
        {
            Author author = _authorRepository.FindById(authorId);
            
            var bookAuthors = bookAuthorRepository.All.Where(ba => ba.AuthorId == author.Id);

            if (author == null)
                return NotFound();

            _coverImageService.DeleteCover(_authorRepository, author);

            bookAuthorRepository.DeleteCollection(bookAuthors);

            _authorRepository.Delete(authorId);

            return RedirectToAction("GetAuthors");
        }
    }
}
