﻿using Bookshelf_FL.ViewModels.AdminViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models.AdminModels
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .EmailAddress().WithMessage("Enter a valid email address.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("The 'Password' field is required")
                .MinimumLength(8).WithMessage("Min password length is 8")
                .Matches("[A-Z]").WithMessage("The password must contain at least one capital letter")
                .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("The password must contain at least one number")
                .Matches("[_\\-!@#$%^&*(),.?\":{}|<>]").WithMessage("The password must contain at least one special character");
        }
    }
}
