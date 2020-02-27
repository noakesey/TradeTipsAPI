using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class EditCommandValidator : AbstractValidator<EditCommand>
    {
        public EditCommandValidator()
        {
            RuleFor(x => x.Stock).NotNull().SetValidator(new StockDataValidator());
        }

        public class StockDataValidator : AbstractValidator<StockSummaryDTO>
        {
            public StockDataValidator()
            {
                RuleFor(x => x.StockId).NotNull().NotEmpty();
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }
    }
}
