using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Articles
{
    public class DetailsQueryValidator : AbstractValidator<DetailsQuery>
    {
        public DetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
