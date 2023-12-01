using Bookshelf_FL.Extensions.Services.AccountServices;
using Bookshelf_FL.Extensions.Validators;
using Bookshelf_FL.ViewModels.AccountViewModels;
using Bookshelf_TL.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookshelf_FL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
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
            [FromServices] IValidator<RegisterViewModel> validator,
            [FromServices] RegistrationService registrationService)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            return await registrationService.RegisterUser(model, this.ModelState);
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
            [FromServices] IValidator<LoginViewModel> validator,
            [FromServices] LoginService loginService)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            return await loginService.LoginUser(model, this.ModelState);
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
            [FromServices] IValidator<ChangePasswordViewModel> validator,
            [FromServices] ChangePasswordService passwordChangeService)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            return await passwordChangeService.ChangePassword(model, this.ModelState);
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
            [FromServices] ForgotPasswordService forgotPasswordService)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            var resultOfRecovery = await forgotPasswordService.ForgotPassword(model, HttpContext);

            if (resultOfRecovery is OkResult)
                return View("ForgotPasswordConfirmation");

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string userEmail, string token)
        {
            if (userEmail == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetForgottenPassword { Email = userEmail, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetForgottenPassword(ResetForgottenPassword model,
            [FromServices] IValidator<ResetForgottenPassword> validator,
            [FromServices] ForgotPasswordService forgotPasswordService)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return View(model);
            }

            var resultOfRecovery = await forgotPasswordService.ResetForgottenPassword(model, ModelState);

            if (resultOfRecovery is OkResult)
                return View("ResetPasswordConfirmation");

            return View(model);
        }
    }
}
