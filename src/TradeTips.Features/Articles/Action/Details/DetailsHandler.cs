using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Articles
{
    public class DetailsHandler : IRequestHandler<DetailsQuery, ArticleEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public DetailsHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleEnvelopeDTO> Handle(DetailsQuery message, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(x => x.Author)
                .Include(x => x.ArticleFavorites)
                .Include(x => x.ArticleTags)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ArticleId == message.Id, cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            //TODO Incorporate these lookups into the above query to avoid 2 trips
            var previousArticle = await _context.Articles
                .Where(a => a.StockId == article.StockId &&
                            a.PublicationDate < article.PublicationDate)
                .OrderBy(a => a.PublicationDate)
                .FirstAsync();

            var articleCount = await _context.Articles
                .Where(a => a.StockId == article.StockId)
                .CountAsync();

            ArticleDetailDTO dto = _mapper.Map<ArticleDetailDTO>(article);
            dto.PreviousPublicationDate = previousArticle.PublicationDate;
            dto.PreviousPrice = (double)previousArticle.Price;
            dto.ArticleCount = articleCount;

            return new ArticleEnvelopeDTO(dto);
        }
    }
}