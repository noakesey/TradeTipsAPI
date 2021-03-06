﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Followers
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(x => x.Username)).NotEmpty();
        }
    }
}
