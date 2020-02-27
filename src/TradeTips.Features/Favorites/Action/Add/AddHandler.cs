using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Features.Articles;
using TradeTips.Domain;
using TradeTips.Security;
using AutoMapper;

namespace TradeTips.Features.Favorites
{
    public class AddHandler : IRequestHandler<AddCommand, ArticleEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public AddHandler(TradeTipsContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<ArticleEnvelopeDTO> Handle(AddCommand message, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(x => x.ArticleId == message.Id, cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

            var favorite = await _context.ArticleFavorites.FirstOrDefaultAsync(x => x.ArticleId == article.ArticleId && x.PersonId == person.PersonId, cancellationToken);

            if (favorite == null)
            {
                favorite = new ArticleFavorite()
                {
                    Article = article,
                    ArticleId = article.ArticleId,
                    Person = person,
                    PersonId = person.PersonId
                };
                await _context.ArticleFavorites.AddAsync(favorite, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var newArticle = await _context.Articles
                .Include(x => x.Author)
                .Include(x => x.ArticleFavorites)
                .Include(x => x.ArticleTags)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ArticleId == article.ArticleId, cancellationToken);

            ArticleDetailDTO dto = _mapper.Map<ArticleDetailDTO>(article);

            return new ArticleEnvelopeDTO(dto);
        }
    }
}