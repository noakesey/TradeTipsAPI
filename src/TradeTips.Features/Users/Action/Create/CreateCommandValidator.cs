using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Users
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
        }

        public class UserDataValidator : AbstractValidator<CreateUserDTO>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }
    }
}
