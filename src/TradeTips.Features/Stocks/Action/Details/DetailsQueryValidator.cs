using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class DetailsQueryValidator : AbstractValidator<DetailsQuery>
    {
        public DetailsQueryValidator()
        {
            RuleFor(x => x.StockId).NotNull().NotEmpty();
        }
    }
}
