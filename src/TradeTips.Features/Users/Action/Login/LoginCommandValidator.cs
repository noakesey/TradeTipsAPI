using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Users
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
        }

        public class UserDataValidator : AbstractValidator<LoginUserDTO>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }
    }
}
