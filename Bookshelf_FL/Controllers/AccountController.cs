using Bookshelf_FL.Extensions.Validators;
using Bookshelf_FL.Extensions.Validators.Models;
using Bookshelf_FL.Models.AccountViewModels;
using Bookshelf_TL.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookshelf_FL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model,
            [FromServices] IValidator<RegisterViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            User user = new User { Email = model.Email, UserName = model.Login };

            var resultOfCreate = await _userManager.CreateAsync(user, model.Password);

            if (resultOfCreate.Succeeded)
            {
                var ageClaim = new Claim(ClaimTypes.DateOfBirth, model.Age.ToString());
                await _userManager.AddClaimAsync(user, ageClaim);

                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultOfCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            string returnUrl = Request.Headers["Referer"];

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return View(new LoginViewModel { ReturnUrl = returnUrl });
            }

            return View(new LoginViewModel { ReturnUrl = @"\Home\Index" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,
            [FromServices] IValidator<LoginViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

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
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірні email та (чи) пароль");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model,
            [FromServices] IValidator<ChangePasswordViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                IdentityResult resultOfChangePass =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (resultOfChangePass.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in resultOfChangePass.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Користувача не знайдено");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model,
            [FromServices] IValidator<ForgotPasswordViewModel> validator,
            [FromServices] IEmailSender emailSender)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userEmail = user.Email, token }, protocol: HttpContext.Request.Scheme);

                await emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string userEmail, string token)
        {
            if (userEmail == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Email = userEmail, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model,
            [FromServices] IValidator<ResetPasswordViewModel> validator)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var resultResetPass = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resultResetPass.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }

                foreach (var error in resultResetPass.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ModelState.AddModelError(string.Empty, "User not found");

            return View(model);
        }


    }
}
