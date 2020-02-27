using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Comments
{
    public class ListQueryHandler : IRequestHandler<ListQuery, CommentsEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public ListQueryHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentsEnvelopeDTO> Handle(ListQuery message, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(x => x.Comments)
                    .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.ArticleId == message.Id, cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            List<CommentDTO> dtos = _mapper.Map<List<CommentDTO>>(article.Comments);

            return new CommentsEnvelopeDTO(dtos);
        }
    }
}