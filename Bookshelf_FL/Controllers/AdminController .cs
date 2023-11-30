using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Bookshelf_TL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bookshelf_FL.Models.AdminViewModels;
using Bookshelf_FL.Extensions.Services;
using Bookshelf_SL.Repositories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL.Models.IntermediateModels;
using FluentValidation;
using FluentValidation.Results;
using Bookshelf_FL.Extensions.Validators;

namespace Bookshelf_FL.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        UserManager<User> _userManager;
        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model,
            [FromServices] IValidator<CreateUserViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            User user = new User { Email = model.Email, UserName = model.Login };

            var resultOfCreation = await _userManager.CreateAsync(user, model.Password);
            if (resultOfCreation.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in resultOfCreation.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Login = user.UserName };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model,
            [FromServices] IValidator<EditUserViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Login;

                var resultOfUpdate = await _userManager.UpdateAsync(user);
                if (resultOfUpdate.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in resultOfUpdate.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, [FromServices] IValidator<ChangePasswordViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var _passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Користувача не знайдено");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(
            string id,
            [FromServices] CoverImageService coverImageService,
            [FromServices] IRepository<User> userRepository,
            [FromServices] IIntermediateRepository<BookUser> bookUserRepository)
        {
            User user = await _userManager.FindByIdAsync(id);

            var bookUsers = bookUserRepository.All.Where(ba => ba.UserId == user.Id);

            if (user == null)
                return NotFound();

            coverImageService.DeleteCover(userRepository, user);

            bookUserRepository.DeleteCollection(bookUsers);

            IdentityResult result = await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}
