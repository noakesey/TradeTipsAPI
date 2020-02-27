using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Comments
{
    public class DeleteHandler : IRequestHandler<DeleteCommand>
    {
        private readonly TradeTipsContext _context;

        public DeleteHandler(TradeTipsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCommand message, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.ArticleId == message.Id, cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            var comment = article.Comments.FirstOrDefault(x => x.CommentId == message.Id);
            if (comment == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Comment = Constants.NOT_FOUND });
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}