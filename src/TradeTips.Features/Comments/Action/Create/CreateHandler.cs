using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Comments
{
    public class CreateHandler : IRequestHandler<CreateCommand, CommentEnvelopeDTO>
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

        public async Task<CommentEnvelopeDTO> Handle(CreateCommand message, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.ArticleId == message.Id, cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            var author = await _context.Persons.FirstAsync(
                x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

            var comment = new Comment()
            {
                Author = author,
                Body = message.Comment.Body,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.Comments.AddAsync(comment, cancellationToken);

            article.Comments.Add(comment);

            await _context.SaveChangesAsync(cancellationToken);

            CommentDTO dto = _mapper.Map<Comment, CommentDTO>(comment);

            return new CommentEnvelopeDTO(dto);
        }
    }
}
