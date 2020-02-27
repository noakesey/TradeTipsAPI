using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Prices
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.StockId).NotNull();
            RuleFor(x => x.Prices).NotNull();
            RuleForEach(x => x.Prices).SetValidator(new PriceDataValidator());
        }

        public class PriceDataValidator : AbstractValidator<PriceDTO>
        {
            public PriceDataValidator()
            {
                RuleFor(x => x.TransDate).NotNull().NotEmpty();
                RuleFor(x => x.Low).GreaterThan(0).NotEmpty();
                RuleFor(x => x.High).GreaterThan(0).NotEmpty();

                RuleFor(x => x.Open).GreaterThan(0).When(x => x.Close > 0);
                RuleFor(x => x.Close).GreaterThan(0).When(x => x.Open > 0);
            }
        }
    }
}
