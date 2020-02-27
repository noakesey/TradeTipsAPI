using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Articles
{
    public class CreateHandler : IRequestHandler<CreateCommand, ArticleEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public CreateHandler(TradeTipsContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<ArticleEnvelopeDTO> Handle(CreateCommand message, CancellationToken cancellationToken)
        {
            if (await _context.Articles.Where(x => x.ArticleId == message.Article.ArticleId)
                .AnyAsync(cancellationToken))
            {
                throw new RestException(HttpStatusCode.BadRequest, 
                    new { ArticleSummaryDTO = Constants.IN_USE });
            }

            var author = await _context.Persons.FirstAsync(
                x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

            var tags = new List<Tag>();

            foreach (var tag in (message.Article.TagList ?? Enumerable.Empty<string>()))
            {
                var t = await _context.Tags.FindAsync(tag);
                if (t == null)
                {
                    t = new Tag()
                    {
                        TagId = tag
                    };

                    await _context.Tags.AddAsync(t, cancellationToken);
                    //save immediately for reuse
                    await _context.SaveChangesAsync(cancellationToken);
                }
                tags.Add(t);
            }

            Article article = _mapper.Map<Article>(message.Article);
            article.Slug = message.Article.Title.GenerateSlug();
            article.UpdatedDate = DateTime.UtcNow;
            article.Author = author;

            await _context.Articles.AddAsync(article, cancellationToken);

            await _context.ArticleTags.AddRangeAsync(tags.Select(x => new ArticleTag()
            {
                Article = article,
                Tag = x
            }), cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            ArticleDetailDTO dto = _mapper.Map<ArticleDetailDTO>(article);

            return new ArticleEnvelopeDTO(dto);
        }
    }
}
