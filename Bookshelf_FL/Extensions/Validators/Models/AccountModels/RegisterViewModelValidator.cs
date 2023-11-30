﻿using Bookshelf_FL.Models.AccountViewModels;
using FluentValidation;

namespace Bookshelf_FL.Extensions.Validators.Models.AccountModels
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("The 'Login' field is required.")
                .Must(x => x.Length <= 30).WithMessage("Max login length is 20");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .EmailAddress().WithMessage("Enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The 'Password' field is required.")
                .MinimumLength(8).WithMessage("Min password length is 8")
                .Matches("[A-Z]").WithMessage("The password must contain at least one capital letter")
                .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("The password must contain at least one number")
                .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("The password must contain at least one special character");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("The 'Password' field is required.")
                .MinimumLength(8).WithMessage("Min password length is 8")
                .Matches("[A-Z]").WithMessage("The password must contain at least one capital letter")
                .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("The password must contain at least one number")
                .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("The password must contain at least one special character")
                .Must((model, confPass) => model.Password == confPass).WithMessage("Passwords do not match");
        }
    }
}
