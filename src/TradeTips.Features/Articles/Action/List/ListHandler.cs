using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Articles
{
    public class ListHandler : IRequestHandler<ListQuery, ArticlesEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public ListHandler(TradeTipsContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<ArticlesEnvelopeDTO> Handle(ListQuery message, CancellationToken cancellationToken)
        {
            IQueryable<Article> queryable = _context.Articles
                .Include(x => x.Author)
                .Include(x => x.ArticleFavorites)
                .Include(x => x.ArticleTags)
                .AsNoTracking();

            if (message.IsFeed && _currentUserAccessor.GetCurrentUsername() != null)
            {
                var currentUser = await _context.Persons.Include(
                    x => x.Following).FirstOrDefaultAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

                queryable = queryable.Where(x => currentUser.Following.Select(
                    y => y.TargetId).Contains(x.Author.PersonId));
            }

            if (!string.IsNullOrWhiteSpace(message.Tag))
            {
                var tag = await _context.ArticleTags.FirstOrDefaultAsync(x => x.TagId == message.Tag, cancellationToken);
                if (tag != null)
                {
                    queryable = queryable.Where(x => x.ArticleTags.Select(y => y.TagId).Contains(tag.TagId));
                }
                else
                {
                    return new ArticlesEnvelopeDTO();
                }
            }

            if (!string.IsNullOrWhiteSpace(message.StockId))
            {
                queryable = queryable.Where(x => x.StockId == message.StockId);
            }

            if (!string.IsNullOrWhiteSpace(message.FavoritedUsername))
            {
                var author = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.FavoritedUsername, cancellationToken);
                if (author != null)
                {
                    queryable = queryable.Where(x => x.ArticleFavorites.Any(y => y.PersonId == author.PersonId));
                }
                else
                {
                    return new ArticlesEnvelopeDTO();
                }
            }

            var articles = await queryable
                .OrderByDescending(x => x.PublicationDate)
                .Skip(message.Offset ?? 0)
                .Take(message.Limit ?? 2000)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<ArticleListDTO>>(articles);

            return new ArticlesEnvelopeDTO()
            {
                Articles = dtos,
                ArticlesCount = queryable.Count()
            };
        }
    }
}