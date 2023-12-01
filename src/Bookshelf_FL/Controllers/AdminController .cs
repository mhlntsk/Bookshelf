using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.Extensions.Validators;
using Bookshelf_FL.ViewModels.AdminViewModels;
using Bookshelf_TL.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookshelf_FL.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AdminService _adminService;
        public AdminController(UserManager<User> userManager, AdminService adminService)
        {
            _userManager = userManager;
            _adminService = adminService;
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

            return await _adminService.Create(model, this.ModelState);
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

            return await _adminService.Edit(model, this.ModelState);
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
        public async Task<IActionResult> ChangePassword(
            ChangePasswordViewModel model, 
            [FromServices] IValidator<ChangePasswordViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);

                return View(model);
            }

            return await _adminService.ChangePassword(model, ModelState, HttpContext);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _adminService.Delete(id);

            if (result is OkResult)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
