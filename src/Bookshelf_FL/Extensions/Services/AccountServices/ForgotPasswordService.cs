using Bookshelf_FL.ViewModels.AccountViewModels;
using Bookshelf_TL.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Bookshelf_FL.Extensions.Services.AccountServices
{
    public class ForgotPasswordService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUrlHelper _urlHelper;

        public ForgotPasswordService(
            UserManager<User> userManager,
            IEmailSender emailSender,
            IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _urlHelper = urlHelper;
        }

        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordViewModel model,
            HttpContext HttpContext)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = _urlHelper.Action("ResetPassword", "Account", new { userEmail = user.Email, token }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return new OkResult();
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> ResetForgottenPassword(
            ResetForgottenPassword model,
            ModelStateDictionary modelState)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var resultResetPass = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resultResetPass.Succeeded)
                {
                    return new OkResult();
                }

                foreach (var error in resultResetPass.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
            }

            modelState.AddModelError(string.Empty, "User not found");

            return new BadRequestResult();
        }
    }
}
