using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Articles
{
    public class EditCommandValidator : AbstractValidator<EditCommand>
    {
        public EditCommandValidator()
        {
            RuleFor(x => x.Article).NotNull();
        }
    }
}
