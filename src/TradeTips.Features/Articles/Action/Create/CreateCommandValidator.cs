using FluentValidation;
using System;

namespace TradeTips.Features.Articles
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.Article).NotNull().SetValidator(new ArticleDataValidator());
        }

        internal class ArticleDataValidator : AbstractValidator<ArticleEditDTO>
        {
            public ArticleDataValidator()
            {
                RuleFor(x => x.ArticleId).NotNull().NotEmpty();
                RuleFor(x => x.Summary).NotNull().NotEmpty();
                RuleFor(x => x.Teaser).NotNull().NotEmpty();
                RuleFor(x => x.Title).NotNull().NotEmpty();
                RuleFor(x => x.Link).NotNull().NotEmpty()
                    .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                    .WithMessage("Link must be a valid Uri");
                RuleFor(x => x.PublicationDate).NotNull().NotEmpty();
            }
        }
    }
}
