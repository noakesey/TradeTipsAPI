using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Comments
{
    public class CommentDataValidator : AbstractValidator<CommentDTO>
    {
        public CommentDataValidator()
        {
            RuleFor(x => x.Body).NotNull().NotEmpty();
        }
    }
}
