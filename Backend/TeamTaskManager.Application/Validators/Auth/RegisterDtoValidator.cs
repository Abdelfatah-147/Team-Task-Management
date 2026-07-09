using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TeamTaskManager.Application.DTOs.Auth;

namespace TeamTaskManager.Application.Validators.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3).MaximumLength(50).Matches("^[a-zA-Z0-9_]*$")
                .WithMessage("Username can only contain letters, numbers, and underscores.");
            
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).Matches("[A-Z]").WithMessage("Password must contain at least on uppercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");
            
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
