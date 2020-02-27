using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Articles
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
