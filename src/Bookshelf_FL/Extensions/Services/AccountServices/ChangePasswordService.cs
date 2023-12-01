using Bookshelf_FL.ViewModels.AccountViewModels;
using Bookshelf_TL.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bookshelf_FL.Extensions.Services.AccountServices
{
    public class ChangePasswordService
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, ModelStateDictionary modelState)
        {
            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                IdentityResult resultOfChangePass =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (resultOfChangePass.Succeeded)
                {
                    return new RedirectToActionResult("Login", "Account", null, false);
                }
                else
                {
                    foreach (var error in resultOfChangePass.Errors)
                    {
                        modelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                modelState.AddModelError(string.Empty, "Користувача не знайдено");
            }

            return new ViewResult();
        }
    }
}
