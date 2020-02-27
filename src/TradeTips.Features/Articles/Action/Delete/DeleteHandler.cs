using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Articles
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
                .FirstOrDefaultAsync(x => x.ArticleId == message.Id, cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
