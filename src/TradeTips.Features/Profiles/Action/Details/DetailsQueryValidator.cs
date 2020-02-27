using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Profiles
{
    public class DetailsQueryValidator : AbstractValidator<DetailsQuery>
    {
        public DetailsQueryValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
        }
    }
}
