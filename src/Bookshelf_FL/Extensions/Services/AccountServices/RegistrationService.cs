using Bookshelf_FL.ViewModels.AccountViewModels;
using Bookshelf_TL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace Bookshelf_FL.Extensions.Services.AccountServices
{
    public class RegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegistrationService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> RegisterUser(RegisterViewModel model, ModelStateDictionary ModelState)
        {
            User user = new User { Email = model.Email, UserName = model.Login };

            var resultOfCreate = await _userManager.CreateAsync(user, model.Password);

            if (resultOfCreate.Succeeded)
            {
                var ageClaim = new Claim(ClaimTypes.DateOfBirth, model.Age.ToString());
                await _userManager.AddClaimAsync(user, ageClaim);

                await _signInManager.SignInAsync(user, false);

                return new RedirectToActionResult("Index", "Home", null, false);
            }
            else
            {
                foreach (var error in resultOfCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return new ViewResult();
        }
    }
}
