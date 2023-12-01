using Bookshelf_FL.ViewModels.AdminViewModels;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_SL.Repositories;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bookshelf_FL.Extensions.Services
{
    public class AdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepository;
        private readonly IIntermediateRepository<BookUser> _bookUserRepository;
        private readonly CoverImageService _coverImageService;

        public AdminService(
            UserManager<User> userManager,
            IRepository<User> userRepository,
            IIntermediateRepository<BookUser> bookUserRepository,
            CoverImageService coverImageService
            )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _bookUserRepository = bookUserRepository;
            _coverImageService = coverImageService;
        }

        public async Task<IActionResult> Create(CreateUserViewModel model, ModelStateDictionary modelState)
        {
            User user = new User { Email = model.Email, UserName = model.Login };

            var resultOfCreation = await _userManager.CreateAsync(user, model.Password);
            if (resultOfCreation.Succeeded)
            {
                return new RedirectToActionResult("Index", "Admin", null, false);
            }
            else
            {
                foreach (var error in resultOfCreation.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
            }

            return new ViewResult();
        }

        public async Task<IActionResult> Edit(EditUserViewModel model, ModelStateDictionary modelState)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Login;

                var resultOfUpdate = await _userManager.UpdateAsync(user);
                if (resultOfUpdate.Succeeded)
                {
                    return new RedirectToActionResult("Index", "Admin", null, false);
                }
                else
                {
                    foreach (var error in resultOfUpdate.Errors)
                    {
                        modelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return new ViewResult();
        }

        public async Task<IActionResult> ChangePassword(
            ChangePasswordViewModel model,
            ModelStateDictionary modelState,
            HttpContext httpContext)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var _passwordHasher =
                    httpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                await _userManager.UpdateAsync(user);
                return new RedirectToActionResult("Index", "Admin", null, false);
            }
            else
            {
                modelState.AddModelError(string.Empty, "Користувача не знайдено");
            }

            return new ViewResult();
        }

        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            var bookUsers = _bookUserRepository.All.Where(ba => ba.UserId == user.Id);

            if (user == null)
                return new NotFoundResult();

            _coverImageService.DeleteCover(_userRepository, user);

            _bookUserRepository.DeleteCollection(bookUsers);

            IdentityResult result = await _userManager.DeleteAsync(user);

            return new RedirectToActionResult("Index", "Admin", null, false);
        }
    }
}
