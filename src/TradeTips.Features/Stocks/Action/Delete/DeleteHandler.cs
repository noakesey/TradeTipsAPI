using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Stocks
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
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.StockId == message.Id, cancellationToken);

            if (stock != null)
            {
                stock.Obsolete = true;

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
