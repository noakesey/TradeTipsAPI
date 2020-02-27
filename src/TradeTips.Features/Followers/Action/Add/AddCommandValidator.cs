using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Followers
{
    public class AddCommandValidator : AbstractValidator<AddCommand>
    {
        public AddCommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(x => x.Username)).NotEmpty();
        }
    }
}
