using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.Extensions.Services.Factories;
using Bookshelf_FL.Extensions.Validators;
using Bookshelf_FL.ViewModels.UserViewModels;
using Bookshelf_SL.Repositories;
using Bookshelf_TL.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bookshelf_FL.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly CoverImageService _coverImageService;
        private readonly FactoryOfBookService _factoryOfBookService;
        private readonly FactoryOfUserService _factoryOfUserService;
        private readonly IValidator<UserEditViewModel> _userEditViewModelValidator;

        public UserController(
            IRepository<User> usersRepository,
            IRepository<Book> bookRepository,
            CoverImageService coverImageService,
            FactoryOfBookService factoryOfBookService,
            FactoryOfUserService factoryOfUserService,
            IValidator<UserEditViewModel> userEditViewModelValidator
)
        {
            _userRepository = usersRepository;
            _bookRepository = bookRepository;
            _coverImageService = coverImageService;
            _factoryOfBookService = factoryOfBookService;
            _factoryOfUserService = factoryOfUserService;
            _userEditViewModelValidator = userEditViewModelValidator;
        }
        public IActionResult GetUsers()
        {
            var users = _userRepository.All;

            var userListViewModels = _factoryOfUserService.UserListViewModels(users);

            return View(userListViewModels);
        }
        public IActionResult SearchUser(string SearchQuery)
        {
            IEnumerable<User> filteredUsers;

            if (SearchQuery == null)
            {
                filteredUsers = _userRepository.All;
            }
            else
            {
                filteredUsers = _userRepository.All
                .Where(user =>
                    (user.FirstName != null && user.FirstName.Contains(SearchQuery))
                    || (user.MiddleName != null && user.MiddleName.Contains(SearchQuery))
                    || (user.LastName != null && user.LastName.Contains(SearchQuery))
                    || (user.UserName != null && user.UserName.Contains(SearchQuery)));
            }

            var userListViewModels = _factoryOfUserService.UserListViewModels(filteredUsers);

            return View(userListViewModels);
        }
        public IActionResult GetUser(string userId)
        {
            var user = _userRepository.FindById(userId);

            if (user == null)
                return NotFound();

            var booksOfUser = _bookRepository.All
                .Where(
                    book => book.BookUsers != null &&
                    book.BookUsers.Any(bookUser => bookUser.UserId == user.Id))
                .ToList();

            var bookListOfUserViewModel = _factoryOfBookService.BookListViewModels(user, booksOfUser);

            var userViewModel = _factoryOfUserService.UserViewModel(user, bookListOfUserViewModel);

            return View(userViewModel);
        }

        public IActionResult Edit(string userId)
        {
            var user = _userRepository.FindById(userId);

            if (user == null)
                return NotFound();

            var userSetUpViewModel = _factoryOfUserService.UserEditViewModel(user);

            return View(userSetUpViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel userSetUpViewModel)
        {
            ValidationResult result = await _userEditViewModelValidator.ValidateAsync(userSetUpViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View("Edit", userSetUpViewModel);
            }

            var user = _userRepository.FindById(userSetUpViewModel.Id);

            if (user == null)
                return NotFound();

            user.FirstName = userSetUpViewModel.FirstName != null ? userSetUpViewModel.FirstName : user.FirstName;
            user.MiddleName = userSetUpViewModel.MiddleName;
            user.LastName = userSetUpViewModel.LastName;
            user.UserName = userSetUpViewModel.UserName;
            user.Email = userSetUpViewModel.Email;
            user.PhoneNumber = userSetUpViewModel.PhoneNumber;
            user.Description = userSetUpViewModel.Description;
            user.BirthDate = userSetUpViewModel.BirthDate;


            if (userSetUpViewModel.CoverImage != null && userSetUpViewModel.CoverImage.Length > 0)
            {
                string folderToSaveName = "UserCovers";
                user.CoverPath = await _coverImageService.AddCover(userSetUpViewModel.CoverImage, folderToSaveName);
            }

            _userRepository.Update(user);

            return RedirectToAction("GetUser", new { userId = user.Id });

        }

        [HttpPost]
        public IActionResult DeleteImage(string id)
        {
            var user = _userRepository.FindById(id);

            _coverImageService.DeleteCover(_userRepository, user);

            return RedirectToAction("Edit", new { userId = id });
        }
    }
}
