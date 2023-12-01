using Bookshelf_FL.ViewModels.AccountViewModels;
using Bookshelf_TL.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Bookshelf_FL.Extensions.Services.AccountServices
{
    public class LoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUrlHelper _urlHelper;

        public LoginService(UserManager<User> userManager, SignInManager<User> signInManager, IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _urlHelper = urlHelper;
        }

        public async Task<IActionResult> LoginUser(LoginViewModel model, ModelStateDictionary ModelState)
        {
            var user = await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            }

            if (user != null)
            {
                var resultOfSignIn = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (resultOfSignIn.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && _urlHelper.IsLocalUrl(model.ReturnUrl))
                    {
                        return new RedirectResult(model.ReturnUrl);
                    }
                    else
                    {
                        return new RedirectToActionResult("Index", "Home", null, false);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірні email та (чи) пароль");
                }
            }

            return new ViewResult();
        }
    }
}
